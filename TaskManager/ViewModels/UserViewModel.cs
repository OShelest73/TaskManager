using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class UserViewModel
{
    [Key]
    public string EmailAddress { get; set; }

    [Display(Name = "ФИО")]
    [Required(ErrorMessage = "ФИО обязательно")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Фио должно содержать от 3 до 50 символов")]
    public string FullName { get; set; }

    [Display(Name = "Категория")]
    [Required(ErrorMessage = "Категория обязательна")]
    public int Category { get; set; }
}
