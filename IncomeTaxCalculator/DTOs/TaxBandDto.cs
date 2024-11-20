using System.ComponentModel.DataAnnotations;

namespace IncomeTaxCalculator.DTOs
{
    public class TaxBandDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal LowerLimit { get; set; }
        public decimal? UpperLimit { get; set; }
        [Required]
        public decimal TaxRate { get; set; }
    }
}