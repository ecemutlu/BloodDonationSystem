using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class BloodRequestDonation
    {
        [ForeignKey("BloodRequest")]
        public int BloodRequestId { get; set; }
        [ForeignKey("Donation")]
        public int DonationId { get; set; }
        public int NoOfUnits { get; set; }
        public DateTime TransactionDateTime { get; set; }

    }   
}
