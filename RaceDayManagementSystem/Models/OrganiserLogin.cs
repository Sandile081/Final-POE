using System.ComponentModel.DataAnnotations;


namespace RaceDayManagementSystem.Models
{
    public class OrganiserLogin
    {
        [Required(ErrorMessage ="Please Enter Username.")]
        public string OrganiserUsername { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please Enter Password")]
        public string OrganiserPassword { get; set; } = string.Empty;
    }
}
