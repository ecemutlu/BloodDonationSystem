using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class BloodRequestDonationDto
    {
        [ForeignKey("BloodRequest")]
        public int BloodRequestId { get; set; }
        [ForeignKey("Donation")]
        public int DonationId { get; set; }
        public int NoOfUnits { get; set; }
        public DateTime TransactionDateTime { get; set; }

    }   
}
