using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FoodJournal.Blazor.Utils;

public static class IngredientNameNormalizer
{
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    public static string NormaliseKey(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;

        // Trim + collapse whitespace
        var s = WhitespaceRegex.Replace(name.Trim(), " ");

        // Normalize Unicode (e.g., composed vs decomposed)
        s = s.Normalize(NormalizationForm.FormKC);

        return s.ToLowerInvariant();
    }

    public static string NormaliseDisplay(string? name)
    {
        var key = NormaliseKey(name);
        return key.Length == 0 ? string.Empty : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(key);
    }
}