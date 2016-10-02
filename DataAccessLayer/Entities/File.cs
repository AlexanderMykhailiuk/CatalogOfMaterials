using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class File
    {
        [Key]
        public int FileID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public byte[] BinaryData { get; set; }

        public virtual Content content { get; set; }
    }
}
