using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class DonationDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Donor")]
        public int DonorId { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public required string BloodType { get; set; }
        public int NoOfUnits { get; set; }
        public required DateTime DonationDateTime { get; set; }
    }
}
