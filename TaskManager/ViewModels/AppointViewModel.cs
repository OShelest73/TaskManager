using System.Collections;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class AppointViewModel
{
    public IEnumerable<UserModel> usersList { get; set; }
    public IEnumerable<UserModel> selectList { get; set; }
    public IEnumerable<UserModel> allUsers { get; set;}
}
