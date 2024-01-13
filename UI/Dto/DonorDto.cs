using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class DonorDto
    {     

        public int Id { get; set; }
        public required string Name { get; set; }        
        public string? Image { get; set; }
		public IFormFile? Photo { get; set; }
		public int CityId { get; set; }       
        public int TownId { get; set; }
        public required string BloodType { get; set; }
        public required string PhoneNo { get; set; }
        public required string Email { get; set; }

    }
}
