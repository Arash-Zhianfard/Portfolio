namespace Abstraction.Interfaces.Services
{    public class CurrencyItem
    {
        public CurrencyItem( string name,int id)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
