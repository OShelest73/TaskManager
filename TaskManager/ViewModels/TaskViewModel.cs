using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class TaskViewModel
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Title of the task")]
    [Required(ErrorMessage = "Title is required")]
    public string TaskName { get; set; }

    [Display(Name = "Description of the task")]
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    [Display(Name = "Deadline of this task")]
    [Required(ErrorMessage = "Deadline is required")]
    public DateTime FinishDate { get; set; }

    [Display(Name = "Status of the task")]
    [Required(ErrorMessage = "Status is required")]
    public int Status { get; set; }

    [Display(Name = "Category of the task")]
    [Required(ErrorMessage = "Category is required")]
    public int Category { get; set; }
}
