using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeDB.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string JobTitle { get; set; }

        [Required, StringLength(150)]
        public string Company { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        // Foreign key relationship
        [ForeignKey(nameof(User))]
        public int UserId_FK { get; set; }

        public User User { get; set; }
    }

}