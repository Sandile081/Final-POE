using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class ParticipantLogin
    {
        [Required(ErrorMessage ="Please Enter Username")]
        public string ParticipantUsername { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please Enter Password")]
        public string ParticipantPassword { get; set; } = string.Empty;
    }
}
