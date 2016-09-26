using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class Content
    {
        [Key]
        public int ContentID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfCreation { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }

        public virtual User user { get; set; }
        public virtual Genre genre { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public Content()
        {
            Files = new List<File>();
        }
    }
}
