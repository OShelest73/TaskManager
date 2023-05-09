using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class TaskModel
{
    [Key]
    public int Id { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime FinishDate { get; set;}

    public StatusModel Status { get; set; }
    public CategoryModel Category { get; set; }
    public WorkspaceModel Workspace { get; set; }
    public UserModel? Author { get; set; }

    public List<UserModel> AppointedUsers { get; set; } = new();

    public TaskModel()
    {
        CreatedDate = DateTime.UtcNow;
        Status = new StatusModel();
        Category = new CategoryModel();
        Workspace = new WorkspaceModel();
        Author = new UserModel();
    }
}
