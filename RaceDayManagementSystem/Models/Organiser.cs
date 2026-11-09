using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string ORname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter Surname")]
        public string ORsurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter Username")]
        public string ORusername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter Password")]
        public string ORpassword { get; set; } = string.Empty;
    }
}
