using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementSystem.Models
{
    public class Categories
    {
        [Key]
        public int CategoriesID { get; set; }

        [Required(ErrorMessage ="Please Enter Age")]
        public int Age { get; set; }

        [Required(ErrorMessage ="Please Enter CategoryType")]
        public string categoryType { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please Enter MinKM")]
        public string MinKM { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter MaxKM")]
        public string MaxKM { get; set; } = string.Empty;

        public int ID { get; set; }
    }
}
