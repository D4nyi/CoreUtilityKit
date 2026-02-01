namespace CoreUtilityKit.UnitTests.DataGenerators.Models;

internal static class UrlHelpersGenerator
{
    internal static IEnumerable<string[]> GenerateParts(int n)
    {
        int boundaries = n - 1;
        int total = (int)Math.Pow(4, boundaries);

        for (int mask = 0; mask < total; mask++)
        {
            string[] parts = Enumerable.Range(0, n).Select(x => $"part{x}").ToArray();
            int value = mask;

            for (int i = 0; i < boundaries; i++)
            {
                int state = value % 4;
                value /= 4;

                bool leftSlash = (state & 1) != 0;
                bool rightSlash = (state & 2) != 0;

                if (leftSlash)
                    parts[i] += "/";

                if (rightSlash)
                    parts[i + 1] = "/" + parts[i + 1];
            }

            yield return parts;
        }
    }

    internal static IEnumerable<string?[]> GenerateThrowsParts(int n)
    {
        int nullIdx = 0;
        for (int i = 0; i < n; i++)
        {
            string?[] parts = Enumerable.Repeat<string?>("", n).ToArray();

            parts[nullIdx++] = null;

            yield return parts;
        }
    }
}
