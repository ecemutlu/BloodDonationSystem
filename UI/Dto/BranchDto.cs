using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class BranchDto
    {        
        public int Id { get; set; }
        public required string Name { get; set; }        
        public int CityId { get; set; }       
        public int TownId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
