using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace ReserveHub.Application.Validators;

public class ValidCountryAttribute : ValidationAttribute
{
    private static readonly Lazy<HashSet<string>> ValidCountries = new Lazy<HashSet<string>>(() =>
    {
        var countries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        // Get all valid countries by using region codes directly
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Where(c => !string.IsNullOrEmpty(c.Name));
        
        foreach (var culture in cultures)
        {
            try
            {
                // Extract region code from culture name (e.g., "en-US" -> "US")
                var parts = culture.Name.Split('-');
                if (parts.Length >= 2)
                {
                    var regionCode = parts[parts.Length - 1];
                    // RegionInfo constructor accepts two-letter ISO country codes
                    var regionInfo = new RegionInfo(regionCode);
                    countries.Add(regionInfo.EnglishName);
                }
            }
            catch
            {
                // Skip cultures that don't have valid region info
                continue;
            }
        }
        
        return countries;
    });

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string countryName)
        {
            var trimmedName = countryName.Trim();
            
            if (ValidCountries.Value.Contains(trimmedName))
                return ValidationResult.Success!;
        }
        return new ValidationResult("Invalid country name.");
    }
}
