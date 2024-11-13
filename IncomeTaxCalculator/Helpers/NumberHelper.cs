namespace IncomeTaxCalculator.Helpers
{
    public static class NumberHelper
    {
        public const decimal PercentageDivider = 100m;

        public static decimal RoundToTwoDecimalPlaces(decimal value)
        {
            return Math.Round(value, 2);
        }

        public static decimal ConvertAnnualToMonthly(decimal annualValue)
        {
            return annualValue / 12;
        }

        public static decimal CalculatePercentage(decimal percentage)
        {
            return percentage / PercentageDivider;
        }
    }
}
