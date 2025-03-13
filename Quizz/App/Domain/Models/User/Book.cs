using System.ComponentModel.DataAnnotations;

namespace Quizz.App.Domain.Models.User
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NameBook {get; set; }

        [Required]
        [MaxLength(100)]
        public string Izdatel { get; set; }

        [Required]
        public int Length { get; set; }

        [Range(0, 1000)]
        public int Age { get; set; }
    }
}
