using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class UserModel
{
    [Key]
    public string EmailAddress { get; set; }
    public string FullName { get; set; }
    public CategoryModel Category { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }

    public virtual List<WorkspaceModel> Workspaces { get; set; } = new();

    public int? TaskId { get; set; }
    public TaskModel Task { get; set; }

    public virtual List<TaskModel> Tasks { get; set;} = new();
}
