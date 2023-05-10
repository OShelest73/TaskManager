using TaskManager.Models;

namespace TaskManager.Data.Services;
public interface INotificationsService
{
    Task CheckNotification(NotificationModel notification);
    Task CreateNotification(string sender, List<UserModel> receivers, string title, string message);
    Task CreateNotification(string sender, UserModel receiver, string title, string message);
    Task DeleteNotifications(string emailAddress);
    Task<NotificationModel> GetNotification(int id);
    Task<IEnumerable<NotificationModel>> GetNotifications(string emailAddress);
}