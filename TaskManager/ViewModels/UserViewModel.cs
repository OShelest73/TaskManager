using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class UserViewModel
{
    [Key]
    public string EmailAddress { get; set; }

    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
    public string FullName { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category is required")]
    public int Category { get; set; }
}
