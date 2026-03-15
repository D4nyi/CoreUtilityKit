namespace CoreUtilityKit.Text;

/// <summary>
/// Provides utility methods for URL-related operations.
/// </summary>
public static class UrlHelper
{
    /// <summary>
    /// Converts a dictionary of string keys and values to a query string.
    /// </summary>
    /// <param name="dictionary">The dictionary containing query parameters.</param>
    /// <returns>A query string representation of the dictionary, or an empty string if the dictionary is null or empty.</returns>
    public static string ToQueryString(Dictionary<string, string>? dictionary)
    {
        if (dictionary is null || dictionary.Count == 0)
        {
            return "";
        }

        ValueStringBuilder sb = new();

        foreach ((string key, string value) in dictionary)
        {
            sb.Append(key);
            sb.Append('=');
            sb.Append(value);
            sb.Append('&');
        }

        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }

    /// <summary>
    /// Converts a dictionary of string keys and <see cref="ISpanFormattable"/> values to a query string.
    /// </summary>
    /// <typeparam name="T">The type of the values, which must implement <see cref="ISpanFormattable"/>.</typeparam>
    /// <param name="dictionary">The dictionary containing query parameters.</param>
    /// <returns>A query string representation of the dictionary, or an empty string if the dictionary is null or empty.</returns>
    public static string ToQueryString<T>(Dictionary<string, T>? dictionary) where T : ISpanFormattable
    {
        if (dictionary is null || dictionary.Count == 0)
        {
            return "";
        }

        ValueStringBuilder sb = new();

        foreach ((string key, T value) in dictionary)
        {
            sb.Append(key);
            sb.Append('=');
            sb.AppendSpanFormattable(value);
            sb.Append('&');
        }

        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }
}
