namespace Zadanie_rekrutacyjne.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int? Count { get; set; } = 0;
        public double share { get; set; } = 0.0;
    }
}
