using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

    }
}
