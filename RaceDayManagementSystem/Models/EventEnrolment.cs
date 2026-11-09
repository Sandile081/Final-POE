using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class EventEnrolment
    {
        [Key]
        public int EventEnrolmentID { get; set; }
        public int EventID { get; set; }
        public int ParticipantID { get; set; }
        [Required(ErrorMessage = "Please Enter EnrolmentStatus")]
        public string EnrolmentStatus { get; set; } = string.Empty;

        
    }
}
