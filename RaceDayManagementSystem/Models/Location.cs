using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        [Required(ErrorMessage ="Please Enter Province")]
        public string Province { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please Enter Town")]
        public string Town { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please Enter Physical Address")]
        public string PhysicalAddress { get; set; } = string.Empty;
    }
}
