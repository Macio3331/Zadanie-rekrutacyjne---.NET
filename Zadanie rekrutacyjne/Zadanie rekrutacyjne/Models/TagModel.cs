using System.ComponentModel.DataAnnotations;

namespace Zadanie_rekrutacyjne.Models
{
    /// <summary>
    /// Model of tag.
    /// </summary>
    public class TagModel
    {
        /// <summary>
        /// ID of the tag.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Name of the tag
        /// </summary>
        [Required]
        public string Name { get; set; } = "";
        /// <summary>
        /// Number of posts linked to this tag.
        /// </summary>
        [Required]
        public int Count { get; set; } = 0;
        /// <summary>
        /// Share of the posts linked to this tag to all population. Value calculated as a percentage.
        /// Share = (Count of the tag) / (Count of all tags stored in database) * 100(%)
        /// </summary>
        public double Share { get; set; } = 0.0;
    }
}
