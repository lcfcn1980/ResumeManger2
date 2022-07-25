using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeManger2.Models
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Range(25,55, ErrorMessage ="Currently, We Have no Position Vacant for Your Age")]
        [DisplayName("Age in Years")]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Qualification { get; set; } = "";

        [Required]
        [Range(1,28, ErrorMessage ="Currently, We have no Positions Vacant for your Experience")]
        [DisplayName("Total Experience in Years")]
        public int TotalExperience { get; set; }

        public virtual List<Experience> Experiences { get; set; } = new List<Experience>();

        public string PhotoUrl { get; set; }

        [Display(Name="Profile Photo")]
        [NotMapped]
        public IFormFile ProfilePhoto { get; set; }

    }
}
