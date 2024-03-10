using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Badil.Backend.Services.Tools
{
    public static partial class Extensions
    {
        public static string NormalizeBrand(this string str)
        {
            var normalizedString = str.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return MyRegex().Replace(result, "");
        }

        [System.Text.RegularExpressions.GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial System.Text.RegularExpressions.Regex MyRegex();

        private static readonly char[] separator = [',', ' '];

        public static string ExtractValue(this string input, string key = "en:")
        {
            var parts = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part.StartsWith(key))
                {
                    return part.Substring(key.Length);
                }
            }

            return string.Empty; // or null, depending on your preference
        }

    }
}
