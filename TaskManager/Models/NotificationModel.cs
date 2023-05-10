using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class NotificationModel
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public bool IsChecked { get; set; } = false;
    public string Sender { get; set; }

    public virtual List<UserModel> Receivers { get; set; } = new();
}
