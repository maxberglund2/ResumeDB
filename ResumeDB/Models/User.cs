using System.ComponentModel.DataAnnotations;

namespace ResumeDB.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string ContactInfo { get; set; }

        // Navigation properties
        public virtual List<Education> Educations { get; set; } = new List<Education>();
        public virtual List<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();

    }
}
