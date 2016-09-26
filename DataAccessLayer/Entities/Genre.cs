using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
        public Genre()
        {
            Contents = new List<Content>();
        }
    }
}
