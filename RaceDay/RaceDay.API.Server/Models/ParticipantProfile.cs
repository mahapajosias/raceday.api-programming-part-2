
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Server.Models
{
    [Table("PARTICIPANT_PROFILES")]
    public class ParticipantProfile
    {
        [Key]
        public int ProfileID { get; set; }

        public int UserID { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(30)]
        public string? Gender { get; set; }

        [StringLength(255)]
        public string? ProfilePictureURL { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}
