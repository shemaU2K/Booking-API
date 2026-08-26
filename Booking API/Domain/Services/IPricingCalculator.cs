namespace BookingAPI.Domain.Services;
/// <summary>
/// Defines a contract for calculating the total price of a booking based on the base price per hour,
/// the start and end times, and the total price of any additional services.
/// </summary>
public interface IPricingCalculator
{
    decimal CalculateTotal(decimal basePricePerHour, DateTime startTime, DateTime endTime, decimal servicesTotalPrice);
}

/// <summary>
/// Calculates the total price for a booking based on the base price per hour, the start and end times,
/// and the total price of any additional services.
/// </summary>
public class PricingCalculator : IPricingCalculator
{
    public decimal CalculateTotal(decimal basePricePerHour, DateTime startTime, DateTime endTime, decimal servicesTotalPrice)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Час початку має бути раніше за час завершення.");

        decimal totalRoomPrice = 0;
        var currentHour = startTime;

        while (currentHour < endTime)
        {
            var hourOfDay = currentHour.Hour;
            decimal multiplier;

            if (hourOfDay >= 6 && hourOfDay < 9)
            {
                multiplier = 0.9m;
            }
            else if (hourOfDay >= 12 && hourOfDay < 14)
            {
                multiplier = 1.15m;
            }
            else if (hourOfDay >= 18 && hourOfDay < 23)
            {
                multiplier = 0.8m;
            }
            else if (hourOfDay >= 9 && hourOfDay < 18)
            {
                multiplier = 1.0m;
            }
            else
            {
                throw new ArgumentException("Бронювання можливе лише з 06:00 до 23:00.");
            }

            var nextHour = currentHour.AddHours(1);
            var timeInThisSlot = nextHour > endTime ? (endTime - currentHour).TotalHours : 1.0;

            totalRoomPrice += basePricePerHour * multiplier * (decimal)timeInThisSlot;
            currentHour = nextHour;
        }

        return totalRoomPrice + servicesTotalPrice;
    }
}