using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class WorkspaceModel
{
    public int Id { get; set; }

    [Display(Name = "Workspace name")]
    [Required(ErrorMessage = "Workspace name is required")]
    public string WorkspaceName { get; set; }

    public virtual List<UserModel> Users { get; set; } = new();
}
