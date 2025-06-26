using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeDB.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string School { get; set; }

        [Required, StringLength(150)]
        public string Degree { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Foreign key relationship
        [ForeignKey(nameof(User))]
        public int UserId_FK { get; set; }

        public User User { get; set; }
    }
}
