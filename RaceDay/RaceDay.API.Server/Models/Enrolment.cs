
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Server.Models
{
    [Table("ENROLMENTS")]
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        [Required]
        [StringLength(30)]
        public string EnrolmentStatus { get; set; } = "Active";
    }
}
