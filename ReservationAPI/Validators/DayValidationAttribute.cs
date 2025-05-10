using System.ComponentModel.DataAnnotations;

public class ValidDayAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();

        var monthProperty = type.GetProperty("Month");
        var yearProperty = type.GetProperty("Year");

        if (monthProperty == null || yearProperty == null)
            return new ValidationResult("Month or Year property not found.");

        int month = (int)(monthProperty.GetValue(instance) ?? 0);
        int year = (int)(yearProperty.GetValue(instance) ?? 0);
        int day = (int)(value ?? 0);

        int maxDays = GetDaysInMonth(month, year);

        if (maxDays == 0)
        {
            return new ValidationResult("Invalid month provided. Must be between 1 and 12!");
        }

        if (day < 1 || day > maxDays)
        {
            return new ValidationResult($"Day must be between 1 and {maxDays} for month {month}, year {year}.");
        }

        return ValidationResult.Success;
    }

    private int GetDaysInMonth(int month, int year)
    {
        return month switch
        {
            2 => IsLeapYear(year) ? 29 : 28,
            4 or 6 or 9 or 11 => 30,
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            _ => 0
        };
    }

    private bool IsLeapYear(int year) =>
        (year % 4 == 0) && (year % 100 != 0 || year % 400 == 0);
}