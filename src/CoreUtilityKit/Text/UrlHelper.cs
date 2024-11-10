namespace CoreUtilityKit.Text;

public static class UrlHelper
{
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