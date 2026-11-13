using System.Text.RegularExpressions;

namespace Market.Application.Common;

/// <summary>
/// Phone number rules: digits with an optional leading "+", 8 to 15 digits.
/// Spaces, dashes and brackets typed by the user are removed before the check and before saving.
/// </summary>
public static class PhoneRules
{
    private static readonly Regex Pattern = new(@"^\+?[0-9]{8,15}$", RegexOptions.Compiled);

    /// <summary>
    /// Keeps only digits and "+", e.g. "+387 61 123-456" becomes "+38761123456".
    /// </summary>
    public static string Normalize(string? phone) =>
        new((phone ?? string.Empty).Where(c => char.IsDigit(c) || c == '+').ToArray());

    public static bool IsValid(string? phone) => Pattern.IsMatch(Normalize(phone));

    public static IRuleBuilderOptions<T, string> ValidPhone<T>(this IRuleBuilderInitial<T, string> rule) =>
        rule
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Broj telefona je obavezan.")
            .Must(IsValid).WithMessage("Broj telefona nije ispravan. Primjer: +38761123456.");
}