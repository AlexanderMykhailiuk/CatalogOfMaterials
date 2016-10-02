using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;

namespace WEBLayer.Models
{
    public class ContentUploadModel
    {   
        [Required(ErrorMessage = "Name of content is requred")]
        [Display(Name = "Name of uploading content")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Year of creation content is requred")]
        [Display(Name = "Year of content creation")]
        [Range(1500, 2016, ErrorMessage ="Not allowed year")]
        public int YearOfCreation { get; set; }

        [Required(ErrorMessage = "Description of content is requred")]
        [Display(Name = "Description of uploading content")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Author of content is requred")]
        [Display(Name = "Author of uploading content")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre of content is requred")]
        [Display(Name = "Genre of uploading content")]
        public int GenreID { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}