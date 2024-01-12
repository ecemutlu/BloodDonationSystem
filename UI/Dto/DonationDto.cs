using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class DonationDto
    {   
        public int Id { get; set; }        
        public int DonorId { get; set; }        
        public int BranchId { get; set; }
        public required string BloodType { get; set; }
        public int NoOfUnits { get; set; }
        public required DateTime DonationDateTime { get; set; }
    }
}
