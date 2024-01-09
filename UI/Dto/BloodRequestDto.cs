using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class BloodRequestDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string BloodType { get; set; }
        public required string Requester { get; set; }
        public required string Email { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [ForeignKey("Town")]
        public int TownId { get; set; }
        public int NoOfUnits { get; set; }
        public required int DurationOfSearch { get; set; }
        public required string Reason { get; set; }
        public required DateTime RequestDateTime { get; set; }
        public required string Status { get; set; }

    }
}
