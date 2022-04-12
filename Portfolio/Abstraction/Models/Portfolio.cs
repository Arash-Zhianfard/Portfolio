namespace Abstraction.Models
{
    public class Portfolio : BaseModel
    {
        public ICollection<Position> Positions { get; set; }
        public ICollection<WatchList> WatchLists { get; set; }
        public User User { get; set; }
        public int PostionId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }

}
