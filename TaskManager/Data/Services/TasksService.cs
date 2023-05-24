using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class TasksService : ITasksService
{
    private readonly AppDBContext _dbContext;
    private readonly INotificationsService _notificationsService;

    public TasksService(AppDBContext dbContext, INotificationsService notificationsService)
    {
        _dbContext = dbContext;
        _notificationsService = notificationsService;
    }

    public async Task<IEnumerable<TaskModel>> GetWorkspaceTasks(int workspaceId)
    {
        var result = await _dbContext.Tasks.Where(t => t.Workspace.Id == workspaceId).ToListAsync();
        return result;
    }

    public async Task Add(TaskModel task)
    {
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TaskModel> FindTask(int id)
    {
        var result = await _dbContext.Tasks.Include(t => t.Category).Include(t => t.Status)
            .Include(t => t.Author).Include(t => t.Workspace).FirstOrDefaultAsync(t => t.Id == id);
        return result;
    }

    public async Task<TaskModel> FindTaskWithUsers(int id)
    {
        var result = await _dbContext.Tasks.Include(t => t.Category).Include(t => t.Status)
            .Include(t => t.Author).Include(t => t.Workspace).Include(t => t.AppointedUsers).FirstOrDefaultAsync(t => t.Id == id);
        return result;
    }

    public async Task AppointUsers(int taskId, List<UserModel> usersList, UserModel initiator)
    {
        var task = await FindTaskWithUsers(taskId);
        task.AppointedUsers = usersList;
        _dbContext.Update(task);
        await _dbContext.SaveChangesAsync();

        List<UserModel> copyToNotify = new List<UserModel>();
        copyToNotify.AddRange(usersList);

        await _notificationsService.CreateNotification(initiator.EmailAddress, copyToNotify,
            $"Вам было назначено новое задание",
            $"Вам было назначено новое задание пользователем {initiator.FullName} (Email для связи - {initiator.EmailAddress}). Пожалуйста, проверьте свой профиль для получения дополнительной информации о задании");
    }

    public async Task Update(TaskModel newTask, UserModel initiator)
    {
        _dbContext.Update(newTask);
        await _dbContext.SaveChangesAsync();

        await _notificationsService.CreateNotification(initiator.EmailAddress, newTask.AppointedUsers,
            $"Ваше задание было изменено",
            $"Ваше задание было изменено пользователем {initiator.FullName} (Email для связи - {initiator.EmailAddress}). Пожалуйста, проверьте свой профиль для получения дополнительной информации о задании");
    }

    public async Task Delete(int id, UserModel initiator)
    {
        var task = await FindTaskWithUsers(id);

        await _notificationsService.CreateNotification(initiator.EmailAddress, task.AppointedUsers,
            $"Ваше задание было удалено",
            $"Ваше задание было удалено пользователем {initiator.FullName} (Email для связи - {initiator.EmailAddress})");

        _dbContext.Remove(task);
        _dbContext.SaveChanges();
    }
}
