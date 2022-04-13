using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
