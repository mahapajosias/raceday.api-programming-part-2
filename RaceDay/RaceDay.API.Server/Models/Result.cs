
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Server.Models
{
    [Table("RESULTS")]
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "EnrolmentID must be greater than 0.")]
        public int EnrolmentID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Finish position must be greater than 0.")]
        public int? FinishPosition { get; set; }

        public DateTime? ResultDate { get; set; }
    }
}
