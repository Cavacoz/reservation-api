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

        if (month > 12 || month < 1)
        {
            return new ValidationResult("Invalid month provided. Must be between 1 and 12!");
        }

        int maxDays = GetDaysInMonth(year, month);

        if (day < 1 || day > maxDays)
        {
            return new ValidationResult($"Day must be between 1 and {maxDays} for month {((MonthDays)month).ToString()}, year {year}.");
        }

        DateOnly reservationDay = new(year, month, day);
        if (reservationDay.CompareTo(DateOnly.FromDateTime(DateTime.Today.AddDays(1))) < 0)
        {
            return new ValidationResult($"Day must be later and not earlier than the current day.");
        }

        return ValidationResult.Success;
    }

    private int GetDaysInMonth(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }
}