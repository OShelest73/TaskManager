using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class TaskViewModel
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Название")]
    [Required(ErrorMessage = "Название обязательно")]
    public string TaskName { get; set; }

    [Display(Name = "Описание")]
    [Required(ErrorMessage = "Описание обязательно")]
    public string Description { get; set; }

    [Display(Name = "Заметки")]
    public string? Notes { get; set; }

    [Display(Name = "Дата завершения")]
    [Required(ErrorMessage = "Дата завершения обязательна")]
    public DateTime FinishDate { get; set; }

    [Display(Name = "Статус")]
    [Required(ErrorMessage = "Статус обязателен")]
    public int Status { get; set; }

    [Display(Name = "Категория")]
    [Required(ErrorMessage = "Категория обязательна")]
    public int Category { get; set; }
}
