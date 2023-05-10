using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class NotificationsService : INotificationsService
{
    private readonly AppDBContext _dbContext;

    public NotificationsService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateNotification(string sender, List<UserModel> receivers, string title, string message)
    {
        var initiator = receivers.FirstOrDefault(u => u.EmailAddress == sender);
        
        if (initiator != null) 
        {
            receivers.Remove(initiator);
        }

        if (receivers.Count() > 0)
        {
            NotificationModel notification = new();
            notification.Sender = sender;
            notification.Receivers = receivers;
            notification.Message = message;
            notification.Title = title;

            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task CreateNotification(string sender, UserModel receiver, string title, string message)
    {
        NotificationModel notification = new();
        notification.Sender = sender;
        notification.Receivers.Add(receiver);
        notification.Message = message;
        notification.Title = title;

        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<NotificationModel>> GetNotifications(string emailAddress)
    {
        var result = await _dbContext.Users.Include(c => c.Notifications).FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        return result.Notifications;
    }

    public async Task<NotificationModel> GetNotification(int id)
    {
        var result = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        return result;
    }

    public async Task CheckNotification(NotificationModel notification)
    {
        notification.IsChecked = true;
        _dbContext.Update(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteNotifications(string emailAddress)
    {
        var result = await _dbContext.Users.Include(c => c.Notifications).FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);

        var uncheckedNotifications = result.Notifications.Where(n => n.IsChecked == false).ToList();
        result.Notifications = uncheckedNotifications;
        await _dbContext.SaveChangesAsync();
    }
}
