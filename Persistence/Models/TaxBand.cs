namespace Persistence.Models
{
    public class TaxBand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int LowerLimit { get; set; }
        public int? UpperLimit { get; set; }
        public int TaxRate { get; set; }
    }
}
