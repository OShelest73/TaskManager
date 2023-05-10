using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface ITasksService
{
    Task<IEnumerable<TaskModel>> GetWorkspaceTasks(int workspaceId);
    Task Add(TaskModel task);
    Task Update(TaskModel newTask, UserModel initiator);
    Task<TaskModel> FindTask(int id);
    Task Delete(int id, UserModel initiator);
    Task<TaskModel> FindTaskWithUsers(int id);
    Task AppointUsers(int taskId, List<UserModel> usersList, UserModel initiator);
}
