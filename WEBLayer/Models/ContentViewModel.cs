using System.ComponentModel.DataAnnotations;

namespace WEBLayer.Models
{
    public class ContentViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Date of creation :")]
        [DataType(DataType.Date)]
        public int YearOfCreation { get; set; }

        [Display(Name = "Description :")]
        public string Description { get; set; }

        [Display(Name = "Author :")]
        public string Author { get; set; }

        public int? ImageID { get; set; }
    }
}