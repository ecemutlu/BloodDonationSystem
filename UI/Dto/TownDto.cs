using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class TownDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }        
        public int CityId { get; set; }
    }
}
