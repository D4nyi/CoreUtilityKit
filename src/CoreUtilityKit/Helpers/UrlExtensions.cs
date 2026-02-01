using System.Diagnostics;
using System.Runtime.CompilerServices;

using CoreUtilityKit.Text;

namespace CoreUtilityKit.Helpers;

public static class UrlExtensions
{
    private const char UrlSeparatorChar = '/';
    private const string UrlSeparatorCharAsString = "/";

    public static string Combine(string path1, string path2)
    {
        ArgumentNullException.ThrowIfNull(path1);
        ArgumentNullException.ThrowIfNull(path2);

        return CombineInternal(path1, path2);
    }

    public static string Combine(string path1, string path2, string path3)
    {
        ArgumentNullException.ThrowIfNull(path1);
        ArgumentNullException.ThrowIfNull(path2);
        ArgumentNullException.ThrowIfNull(path3);

        return CombineInternal(path1, path2, path3);
    }

    public static string Combine(string path1, string path2, string path3, string path4)
    {
        ArgumentNullException.ThrowIfNull(path1);
        ArgumentNullException.ThrowIfNull(path2);
        ArgumentNullException.ThrowIfNull(path3);
        ArgumentNullException.ThrowIfNull(path4);

        return CombineInternal(path1, path2, path3, path4);
    }

    public static string Combine(params string[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);

        int maxSize = 0;

        // We have two passes, the first calculates how large a buffer to allocate and does some precondition
        // checks on the paths passed in.  The second actually does the combination.

        for (int i = 0; i < paths.Length - 1; i++)
        {
            string path = paths[i];
            string nextPath = paths[i + 1];

            ArgumentNullException.ThrowIfNull(path, nameof(paths));
            ArgumentNullException.ThrowIfNull(nextPath, nameof(paths));

            if (path.Length == 0)
            {
                continue;
            }

            if (nextPath.Length == 0)
            {
                i++;
                continue;
            }

            maxSize += path.Length;

            bool currentEndsSeparator = IsUrlSeparator(path[^1]);
            bool nextStartsSeparator = IsUrlSeparator(nextPath[0]);

            if (currentEndsSeparator && nextStartsSeparator)
            {
                maxSize--;
            }
            else if (!currentEndsSeparator && !nextStartsSeparator)
            {
                maxSize++;
            }
        }

        maxSize += paths[^1].Length;

        ValueStringBuilder builder = maxSize <= 260
            ? new ValueStringBuilder(stackalloc char[maxSize])
            : new ValueStringBuilder(maxSize);

        foreach (string path in paths)
        {
            if (path.Length == 0)
            {
                continue;
            }

            if (builder.Length != 0)
            {
                bool builderEndsSeparator = IsUrlSeparator(builder[^1]);
                bool currentStartsSeparator = IsUrlSeparator(path[0]);

                if (builderEndsSeparator && currentStartsSeparator)
                {
                    builder.Length--;
                }
                else if (!builderEndsSeparator && !currentStartsSeparator)
                {
                    builder.Append(UrlSeparatorChar);
                }
            }

            builder.Append(path);
        }

        return builder.ToString();
    }

    private static string CombineInternal(string first, string second)
    {
        if (String.IsNullOrEmpty(first))
            return second;

        if (String.IsNullOrEmpty(second))
            return first;

        return JoinInternal(first.AsSpan(), second.AsSpan());
    }

    private static string CombineInternal(string first, string second, string third)
    {
        if (String.IsNullOrEmpty(first))
            return CombineInternal(second, third);
        if (String.IsNullOrEmpty(second))
            return CombineInternal(first, third);
        if (String.IsNullOrEmpty(third))
            return CombineInternal(first, second);

        return JoinInternal(first.AsSpan(), second.AsSpan(), third.AsSpan());
    }

    private static string CombineInternal(string first, string second, string third, string fourth)
    {
        if (string.IsNullOrEmpty(first))
            return CombineInternal(second, third, fourth);
        if (string.IsNullOrEmpty(second))
            return CombineInternal(first, third, fourth);
        if (string.IsNullOrEmpty(third))
            return CombineInternal(first, second, fourth);
        if (string.IsNullOrEmpty(fourth))
            return CombineInternal(first, second, third);

        return JoinInternal(first.AsSpan(), second.AsSpan(), third.AsSpan(), fourth.AsSpan());
    }

    private static string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0, "should have dealt with empty paths");

        bool firstEndsSeparator = IsUrlSeparator(first[^1]);
        bool secondStartsSeparator = IsUrlSeparator(second[0]);

        if (firstEndsSeparator && secondStartsSeparator)
        {
            return String.Concat(first, second[1..]);
        }

        return firstEndsSeparator || secondStartsSeparator
            ? String.Concat(first, second)
            : String.Concat(first, UrlSeparatorCharAsString, second);
    }

    private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0 && third.Length > 0, "should have dealt with empty paths");

        bool firstEndHasSeparator = IsUrlSeparator(first[^1]);

        bool secondStartsSeparator = IsUrlSeparator(second[0]);
        bool secondEndsSeparator = IsUrlSeparator(second[^1]);

        bool thirdStartsSeparator = IsUrlSeparator(third[0]);

        if (firstEndHasSeparator && secondStartsSeparator)
        {
            second = second[1..];
        }

        if (secondEndsSeparator && thirdStartsSeparator)
        {
            third = third[1..];
        }

        JoinInternalState state = new()
        {
            ReadOnlySpanPtr1 = (IntPtr)(&first),
            ReadOnlySpanPtr2 = (IntPtr)(&second),
            ReadOnlySpanPtr3 = (IntPtr)(&third),
            NeedSeparator1 = firstEndHasSeparator || secondStartsSeparator ? (byte)0 : (byte)1,
            NeedSeparator2 = secondEndsSeparator || thirdStartsSeparator ? (byte)0 : (byte)1
        };

        return string.Create(
            first.Length + second.Length + third.Length + state.NeedSeparator1 + state.NeedSeparator2,
            state,
            static (destination, state) =>
            {
                ReadOnlySpan<char> first = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr1;
                first.CopyTo(destination);
                destination = destination[first.Length..];

                if (state.NeedSeparator1 != 0)
                {
                    destination[0] = UrlSeparatorChar;
                    destination = destination[1..];
                }

                ReadOnlySpan<char> second = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr2;
                second.CopyTo(destination);
                destination = destination[second.Length..];

                if (state.NeedSeparator2 != 0)
                {
                    destination[0] = UrlSeparatorChar;
                    destination = destination[1..];
                }

                ReadOnlySpan<char> third = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr3;
                Debug.Assert(third.Length == destination.Length);
                third.CopyTo(destination);
            });
    }

    private static unsafe string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third, ReadOnlySpan<char> fourth)
    {
        Debug.Assert(first.Length > 0 && second.Length > 0 && third.Length > 0 && fourth.Length > 0, "should have dealt with empty paths");

        bool firstEndHasSeparator = IsUrlSeparator(first[^1]);

        bool secondStartsSeparator = IsUrlSeparator(second[0]);
        bool secondEndsSeparator = IsUrlSeparator(second[^1]);

        bool thirdStartsSeparator = IsUrlSeparator(third[0]);
        bool thirdEndsSeparator = IsUrlSeparator(third[^1]);

        bool fourthStartsSeparator = IsUrlSeparator(fourth[0]);

        if (firstEndHasSeparator && secondStartsSeparator)
        {
            second = second[1..];
        }

        if (secondEndsSeparator && thirdStartsSeparator)
        {
            third = third[1..];
        }

        if (thirdEndsSeparator && fourthStartsSeparator)
        {
            fourth = fourth[1..];
        }

        JoinInternalState state = new()
        {
            ReadOnlySpanPtr1 = (IntPtr)(&first),
            ReadOnlySpanPtr2 = (IntPtr)(&second),
            ReadOnlySpanPtr3 = (IntPtr)(&third),
            ReadOnlySpanPtr4 = (IntPtr)(&fourth),
            NeedSeparator1 = firstEndHasSeparator || secondStartsSeparator ? (byte)0 : (byte)1,
            NeedSeparator2 = secondEndsSeparator || thirdStartsSeparator ? (byte)0 : (byte)1,
            NeedSeparator3 = thirdEndsSeparator || fourthStartsSeparator ? (byte)0 : (byte)1
        };

        return String.Create(
            first.Length + second.Length + third.Length + fourth.Length + state.NeedSeparator1 + state.NeedSeparator2 + state.NeedSeparator3,
            state,
            static (destination, state) =>
            {
                ReadOnlySpan<char> first = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr1;
                first.CopyTo(destination);
                destination = destination[first.Length..];

                if (state.NeedSeparator1 != 0)
                {
                    destination[0] = UrlSeparatorChar;
                    destination = destination[1..];
                }

                ReadOnlySpan<char> second = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr2;
                second.CopyTo(destination);
                destination = destination[second.Length..];

                if (state.NeedSeparator2 != 0)
                {
                    destination[0] = UrlSeparatorChar;
                    destination = destination[1..];
                }

                ReadOnlySpan<char> third = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr3;
                third.CopyTo(destination);
                destination = destination[third.Length..];

                if (state.NeedSeparator3 != 0)
                {
                    destination[0] = UrlSeparatorChar;
                    destination = destination[1..];
                }

                ReadOnlySpan<char> fourth = *(ReadOnlySpan<char>*)state.ReadOnlySpanPtr4;
                Debug.Assert(fourth.Length == destination.Length);
                fourth.CopyTo(destination);
            });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsUrlSeparator(char ch) => ch == UrlSeparatorChar;

    private struct JoinInternalState // used to avoid rooting ValueTuple`7
    {
        public IntPtr ReadOnlySpanPtr1, ReadOnlySpanPtr2, ReadOnlySpanPtr3, ReadOnlySpanPtr4;
        public byte NeedSeparator1, NeedSeparator2, NeedSeparator3;
    }
}
