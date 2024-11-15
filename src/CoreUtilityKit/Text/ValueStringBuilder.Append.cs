using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using CoreUtilityKit.Helpers;

namespace CoreUtilityKit.Text;

public ref partial struct ValueStringBuilder
{
    public void Insert(int index, char value, int count)
    {
        if (_pos > _chars.Length - count)
        {
            Grow(count);
        }

        int remaining = _pos - index;
        _chars.Slice(index, remaining).CopyTo(_chars[(index + count)..]);
        _chars.Slice(index, count).Fill(value);
        _pos += count;
    }

    public void Insert(int index, string? s)
    {
        if (s is null)
        {
            return;
        }

        int count = s.Length;

        if (_pos > (_chars.Length - count))
        {
            Grow(count);
        }

        int remaining = _pos - index;
        _chars.Slice(index, remaining).CopyTo(_chars[(index + count)..]);
        s.CopyTo(_chars[index..]);

        _pos += count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c)
    {
        int pos = _pos;
        Span<char> chars = _chars;
        if ((uint)pos < (uint)chars.Length)
        {
            chars[pos] = c;
            _pos = pos + 1;
        }
        else
        {
            GrowAndAppend(c);
        }
    }

    public void Append(char c, int count)
    {
        if (_pos > _chars.Length - count)
        {
            Grow(count);
        }

        Span<char> dst = _chars.Slice(_pos, count);
        for (int i = 0; i < dst.Length; i++)
        {
            dst[i] = c;
        }

        _pos += count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string? s)
    {
        if (s is null)
        {
            return;
        }

        int pos = _pos;
        if (s.Length == 1 && (uint)pos < (uint)_chars.Length) // very common case, e.g. appending strings from NumberFormatInfo like separators, percent symbols, etc.
        {
            _chars[pos] = s[0];
            _pos = pos + 1;
        }
        else
        {
            if (pos > _chars.Length - s.Length)
            {
                Grow(s.Length);
            }

            s.AsSpan().CopyTo(_chars[pos..]);

            _pos += s.Length;
        }
    }

    [ExcludeFromCodeCoverage(Justification = "Too complex to test and it is copied from Microsoft")]
    public void AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, ReadOnlySpan<object?> args)
    {
        ArgumentNullException.ThrowIfNull(format);

        // Undocumented exclusive limits on the range for Argument Hole Index and Argument Hole Alignment.
        const int IndexLimit = 1_000_000; // Note:            0 <= ArgIndex < IndexLimit
        const int WidthLimit = 1_000_000; // Note:  -WidthLimit <  ArgAlign < WidthLimit

        // Query the provider (if one was supplied) for an ICustomFormatter. If there is one,
        // it needs to be used to transform all arguments.
        ICustomFormatter? cf = (ICustomFormatter?)provider?.GetFormat(typeof(ICustomFormatter));

        // Repeatedly find the next hole and process it.
        int pos = 0;
        while (true)
        {
            // Skip until either the end of the input or the first unescaped opening brace, whichever comes first.
            // Along the way we need to also unescape escaped closing braces.
            char ch;
            while (true)
            {
                // Find the next brace. If there isn't one, the remainder of the input is text to be appended, and we're done.
                if ((uint)pos >= (uint)format.Length)
                {
                    return;
                }

                ReadOnlySpan<char> remainder = format.AsSpan(pos);
                int countUntilNextBrace = remainder.IndexOfAny('{', '}');
                if (countUntilNextBrace < 0)
                {
                    Append(remainder);
                    return;
                }

                // Append the text until the brace.
                Append(remainder[..countUntilNextBrace]);
                pos += countUntilNextBrace;

                // Get the brace.  It must be followed by another character, either a copy of itself in the case of being
                // escaped, or an arbitrary character that's part of the hole in the case of an opening brace.
                char brace = format[pos];
                ch = MoveNext(format, ref pos);
                if (brace == ch)
                {
                    Append(ch);
                    pos++;
                    continue;
                }

                // This wasn't an escape, so it must be an opening brace.
                if (brace != '{')
                {
                    ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_UnexpectedClosingBrace);
                }

                // Proceed to parse the hole.
                break;
            }

            // We're now positioned just after the opening brace of an argument hole, which consists of
            // an opening brace, an index, an optional width preceded by a comma, and an optional format
            // preceded by a colon, with arbitrary amounts of spaces throughout.
            int width = 0;
            bool leftJustify = false;
            ReadOnlySpan<char> itemFormatSpan = default; // used if itemFormat is null

            // First up is the index parameter, which is of the form:
            //     at least on digit
            //     optional any number of spaces
            // We've already read the first digit into ch.
            Debug.Assert(format[pos - 1] == '{');
            Debug.Assert(ch != '{');
            int index = ch - '0';
            if ((uint)index >= 10u)
            {
                ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_ExpectedAsciiDigit);
            }

            // Common case is a single digit index followed by a closing brace.  If it's not a closing brace,
            // proceed to finish parsing the full hole format.
            ch = MoveNext(format, ref pos);
            if (ch != '}')
            {
                // Continue consuming optional additional digits.
                while (Char.IsAsciiDigit(ch) && index < IndexLimit)
                {
                    index = (index * 10) + ch - '0';
                    ch = MoveNext(format, ref pos);
                }

                // Consume optional whitespace.
                while (ch == ' ')
                {
                    ch = MoveNext(format, ref pos);
                }

                // Parse the optional alignment, which is of the form:
                //     comma
                //     optional any number of spaces
                //     optional -
                //     at least one digit
                //     optional any number of spaces
                if (ch == ',')
                {
                    // Consume optional whitespace.
                    do
                    {
                        ch = MoveNext(format, ref pos);
                    }
                    while (ch == ' ');

                    // Consume an optional minus sign indicating left alignment.
                    if (ch == '-')
                    {
                        leftJustify = true;
                        ch = MoveNext(format, ref pos);
                    }

                    // Parse alignment digits. The read character must be a digit.
                    width = ch - '0';
                    if ((uint)width >= 10u)
                    {
                        ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_ExpectedAsciiDigit);
                    }

                    ch = MoveNext(format, ref pos);
                    while (Char.IsAsciiDigit(ch) && width < WidthLimit)
                    {
                        width = (width * 10) + ch - '0';
                        ch = MoveNext(format, ref pos);
                    }

                    // Consume optional whitespace
                    while (ch == ' ')
                    {
                        ch = MoveNext(format, ref pos);
                    }
                }

                // The next character needs to either be a closing brace for the end of the hole,
                // or a colon indicating the start of the format.
                if (ch != '}')
                {
                    if (ch != ':')
                    {
                        // Unexpected character
                        ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_UnclosedFormatItem);
                    }

                    // Search for the closing brace; everything in between is the format,
                    // but opening braces aren't allowed.
                    int startingPos = pos;
                    while (true)
                    {
                        ch = MoveNext(format, ref pos);

                        if (ch == '}')
                        {
                            // Argument hole closed
                            break;
                        }

                        if (ch == '{')
                        {
                            // Braces inside the argument hole are not supported
                            ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_UnclosedFormatItem);
                        }
                    }

                    startingPos++;
                    itemFormatSpan = format.AsSpan(startingPos, pos - startingPos);
                }
            }

            // Construct the output for this arg hole.
            Debug.Assert(format[pos] == '}');
            pos++;
            string? s = null;
            string? itemFormat = null;

            if ((uint)index >= (uint)args.Length)
            {
                ThrowHelpers.ThrowFormatIndexOutOfRange();
            }

            object? arg = args[index];

            if (cf != null)
            {
                if (!itemFormatSpan.IsEmpty)
                {
                    itemFormat = new string(itemFormatSpan);
                }

                s = cf.Format(itemFormat, arg, provider);
            }

            if (s == null)
            {
                // If arg is ISpanFormattable and the beginning doesn't need padding,
                // try formatting it into the remaining current chunk.
                if ((leftJustify || width == 0) &&
                    arg is ISpanFormattable spanFormattableArg &&
                    spanFormattableArg.TryFormat(_chars[_pos..], out int charsWritten, itemFormatSpan, provider))
                {
                    _pos += charsWritten;

                    // Pad the end, if needed.
                    if (leftJustify && width > charsWritten)
                    {
                        Append(' ', width - charsWritten);
                    }

                    // Continue to parse other characters.
                    continue;
                }

                // Otherwise, fallback to trying IFormattable or calling ToString.
                if (arg is IFormattable formattableArg)
                {
                    if (itemFormatSpan.Length != 0)
                    {
                        itemFormat ??= new string(itemFormatSpan);
                    }

                    s = formattableArg.ToString(itemFormat, provider);
                }
                else
                {
                    s = arg?.ToString();
                }

                s ??= String.Empty;
            }

            // Append it to the final output of the Format String.
            if (width <= s.Length)
            {
                Append(s);
            }
            else if (leftJustify)
            {
                Append(s);
                Append(' ', width - s.Length);
            }
            else
            {
                Append(' ', width - s.Length);
                Append(s);
            }

            // Continue parsing the rest of the format string.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char MoveNext(string format, ref int pos)
        {
            pos++;
            if ((uint)pos >= (uint)format.Length)
            {
                ThrowHelpers.ThrowFormatInvalidString(pos, ExceptionResource.Format_UnclosedFormatItem);
            }

            return format[pos];
        }
    }

    public void Append(scoped ReadOnlySpan<char> value)
    {
        int pos = _pos;
        if (pos > _chars.Length - value.Length)
        {
            Grow(value.Length);
        }

        value.CopyTo(_chars[_pos..]);
        _pos += value.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AppendSpan(int length)
    {
        int origPos = _pos;
        if (origPos > _chars.Length - length)
        {
            Grow(length);
        }

        _pos = origPos + length;
        return _chars.Slice(origPos, length);
    }

    public void AppendSpanFormattable<T>(T value, string? format = null, IFormatProvider? provider = null) where T : ISpanFormattable
    {
        if (value.TryFormat(_chars[_pos..], out int charsWritten, format.AsSpan(), provider))
        {
            _pos += charsWritten;
        }
        else
        {
            Append(value.ToString(format, provider));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(Rune rune)
    {
        int pos = _pos;
        Span<char> chars = _chars;
        if ((uint)(pos + 1) < (uint)chars.Length)
        {
            if (rune.Value <= 0xFFFF)
            {
                chars[pos] = (char)rune.Value;
                _pos = pos + 1;
            }
            else
            {
                chars[pos] = (char)((rune.Value + ((0xD800u - 0x40u) << 10)) >> 10);
                chars[pos + 1] = (char)((rune.Value & 0x3FFu) + 0xDC00u);
                _pos = pos + 2;
            }
        }
        else if (rune.Value <= 0xFFFF)
        {
            Append((char)rune.Value);
        }
        else
        {
            Grow(2);
            Append(rune);
        }
    }
}
