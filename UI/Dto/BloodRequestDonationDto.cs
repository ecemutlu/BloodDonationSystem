using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class BloodRequestDonationDto
    {        
        public int BloodRequestId { get; set; }        
        public int DonationId { get; set; }
        public int NoOfUnits { get; set; }
        public DateTime TransactionDateTime { get; set; }

    }   
}
