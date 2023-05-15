using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class WorkspaceModel
{
    public int Id { get; set; }

    [Display(Name = "Название рабочей области")]
    [Required(ErrorMessage = "Название рабочей области обязательно")]
    public string WorkspaceName { get; set; }

    public virtual List<UserModel> Users { get; set; } = new();
}
