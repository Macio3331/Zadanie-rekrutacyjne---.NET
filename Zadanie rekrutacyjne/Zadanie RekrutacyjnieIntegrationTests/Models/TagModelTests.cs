using System.ComponentModel.DataAnnotations;

namespace Zadanie_rekrutacyjne.Models
{
    public class TagModelTests
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public int Count { get; set; } = 0;
        public double Share { get; set; } = 0.0;
    }
}
