namespace Abstraction.Models
{
    public class VwdResponse
    {
        public string VwdKey { get; set; }
        public string Name { get; set; }
        public string Isin { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public object Close { get; set; }
        public int Volume { get; set; }
        public double PreviousClose { get; set; }
        public DateTime PreviousCloseTime { get; set; }
    }
}
