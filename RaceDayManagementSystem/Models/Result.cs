using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }
        [Required(ErrorMessage ="Please Enter Time")]
        public string Time { get; set; } = string.Empty;
        [Required(ErrorMessage ="Enter The Finishing Position")]
        public string FinishingPosition { get; set; } = string.Empty;
        public int OrganiserID { get; set; }
        public int ParticipantID { get; set; }
    }
}
