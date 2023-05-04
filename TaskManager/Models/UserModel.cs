using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class UserModel
{
    [Key]
    public string EmailAddress { get; set; }
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
    public string FullName { get; set; }
    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category is required")]
    public CategoryModel Category { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }

    public virtual List<WorkspaceModel> Workspaces { get; set; } = new();

    public int? TaskId { get; set; }
}
