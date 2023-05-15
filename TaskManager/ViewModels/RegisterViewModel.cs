using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email адрес")]
    [Required(ErrorMessage = "Email адрес обязателен")]
    [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный Email адрес")]
    public string EmailAddress { get; set; }

    [Display(Name = "ФИО")]
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "ФИО должно иметь от 3 до 50 символов")]
    public string FullName { get; set; }

    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен иметь от 6 до 50 символов")]
    public string Password { get; set; }

    [Display(Name = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Некорректный пароль")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Категория")]
    [Required(ErrorMessage = "Категория обязательна")]
    public int? CategoryId { get; set; }

    public CategoryModel? Category { get; set; }
}
