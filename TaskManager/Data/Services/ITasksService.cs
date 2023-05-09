using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface ITasksService
{
    Task<IEnumerable<TaskModel>> GetWorkspaceTasks(int workspaceId);
    Task Add(TaskModel task);
    Task Update(TaskModel newTask);
    Task<TaskModel> FindTask(int id);
    Task Delete(int id);
    Task<TaskModel> FindTaskWithUsers(int id);
    Task AppointUsers(int taskId, List<UserModel> usersList);
}
