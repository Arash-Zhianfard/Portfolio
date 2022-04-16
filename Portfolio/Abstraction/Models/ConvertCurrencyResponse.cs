using System.Text.Json.Serialization;

namespace Abstraction.Models
{
    public class ConvertCurrencyResponse
    {
        [JsonIgnore]
        public double Value => USD_EUR + TJS_EUR + TTD_EUR + EUR_EUR;
        public double USD_EUR { get; set; }
        public double TJS_EUR { get; set; }
        public double TTD_EUR { get; set; }
        public double EUR_EUR { get; set; }
    }
}
