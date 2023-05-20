using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data.Services;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationsService _notificationService;
    private readonly ITasksService _tasksService;

    public NotificationsController(INotificationsService notificationService, ITasksService tasksService)
    {
        _notificationService = notificationService;
        _tasksService = tasksService;
    }

    public async Task<IActionResult> Index()
    {
        var notifications = await _notificationService.GetNotifications(User.Identity.Name);

        notifications = notifications.OrderByDescending(n => n.CreationDate);

        return View(notifications);
    }

    public async Task<IActionResult> Details(int id)
    {
        var notification = await _notificationService.GetNotification(id);
        await _notificationService.CheckNotification(notification);
        return View(notification);
    }

    public async Task<IActionResult> DeleteChecked()
    {
        await _notificationService.DeleteNotifications(User.Identity.Name);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> NotifyCompletion(int taskId)
    {
        var task = await _tasksService.FindTask(taskId);

        await _notificationService.CreateNotification(User.Identity.Name, task.Author, 
            "Завершение задания", $"Пользователь {User.Identity.Name} завершил задание {task.TaskName}. Для получения информации о задании зайдите в рабочее пространство {task.Workspace.WorkspaceName}");

        return RedirectToAction("Profile", "Users", new { id = User.Identity.Name});
    }
}
