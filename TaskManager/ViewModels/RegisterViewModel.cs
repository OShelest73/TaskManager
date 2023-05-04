using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please, enter correct email address")]
    public string EmailAddress { get; set; }

    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
    public string FullName { get; set; }

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
    public string Password { get; set; }

    [Display(Name = "Confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Incorrect password")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category is required")]
    public int? CategoryId { get; set; }

    public CategoryModel? Category { get; set; }
}
