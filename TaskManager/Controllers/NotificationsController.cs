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

    public NotificationsController(INotificationsService notificationService)
    {
        _notificationService = notificationService;
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
}
