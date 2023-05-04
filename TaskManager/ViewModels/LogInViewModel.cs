using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels;

public class LogInViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please, enter correct email address")]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
