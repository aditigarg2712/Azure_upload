using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Upload_img_db.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string Title { get; set; }

        [DisplayName("Image Name")]
        public string ImageName { get; set; }
        // edit start
       // [NotMapped]
        //[DisplayName("Upload File")]
       // public IFormFile ImageFile { get; set; }
    }
}
