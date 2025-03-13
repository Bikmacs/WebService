using System.ComponentModel.DataAnnotations;

namespace Quizz.App.Domain.Models.User;

public class Person
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }
    
    [Range(0, 100)]
    public int Age { get; set; }
}