using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public class BaseModel
    {
        [Key]
        public long Id { get; set; }
    }
}
