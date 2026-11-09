using System.ComponentModel.DataAnnotations;


namespace RaceDayManagementSystem.Models
{
    public class Events
    {
      

        public int EventID { get; set; }

        [Required(ErrorMessage = "Please Event name")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Date")]
        public string Date { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Distance")]
        public string Distance { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter EventType")]
        public string EventType { get; set; } = string.Empty;
        public int OrganiserID { get; set; }
        public int CategoriesID { get; set; }
        public int LocationID { get; set; }

        public int NumberOfEnrolment { get; set; }

        public string BannerUrl { get; set; } = string.Empty;





    }
}
