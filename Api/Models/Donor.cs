using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Donor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }        
        public string? Image { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [ForeignKey("Town")]
        public int TownId { get; set; }
        public required string BloodType { get; set; }
        public required string PhoneNo { get; set; }
        public required string Email { get; set; }

    }
}
