using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [ForeignKey("Town")]
        public int TownId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
