using System.Security.Cryptography.X509Certificates;

namespace Abstraction.Models
{
    public class Stock : BaseModel
    {
        public string Isin { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public ICollection<Position>? Positions { get; set; }

        public int CurrentAssetContract
        {
            get
            {
                if (Positions == null || !Positions.Any()) return 0;
                return Positions.Where(x => x.TransactionType == TransactionType.Buy).Sum(x => x.Contract)
                       - Positions.Where(x => x.TransactionType == TransactionType.Sell).Sum(x => x.Contract);

            }
        }

    }
}
