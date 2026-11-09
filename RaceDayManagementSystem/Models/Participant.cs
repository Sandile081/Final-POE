using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }

        [Required(ErrorMessage ="Please Enter Name")]
        public string Pname { get; set; } = string.Empty;


        [Required(ErrorMessage ="Please Enter Surname")]
        public string Psurname { get; set; } = string.Empty;


        [Required(ErrorMessage ="Please Enter Username")]
        public string Pusername { get; set; } = string.Empty;


        [Required(ErrorMessage ="Please Enter Password")]
        public string Ppassword { get; set; } = string.Empty;

        public string SelectedCatagory { get; set; } = string.Empty;

        public string EnrolmentStatus { get; set; } = string.Empty;

    }
}
