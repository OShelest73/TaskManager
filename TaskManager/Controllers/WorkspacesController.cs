using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Services;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;

[Authorize]
public class WorkspacesController : Controller
{
    private readonly IWorkspacesService _workspacesService;
    private readonly IUsersService _usersService;
    private readonly ITasksService _tasksService;
    private readonly ICategoriesService _categoriesService;

    public WorkspacesController(IWorkspacesService workspacesService, IUsersService usersService, ICategoriesService categoriesService, ITasksService tasksService)
    {
        _workspacesService = workspacesService;
        _usersService = usersService;
        _categoriesService = categoriesService;
        _tasksService = tasksService;
    }

    public async Task<IActionResult> Index()
    {
        var workspaces = await _usersService.GetUsersWorkspaces(User.Identity.Name);
        return View(workspaces);
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkspaceModel workspace)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _usersService.GetByEmail(User.Identity.Name);
        await _workspacesService.Add(workspace, user, user);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> InviteUsers(int workspace, int? categoryId, string searchString)
    {
        var invitedUsers = await _workspacesService.GetInvitedUsers(workspace);
        var usersToInvite = await _usersService.GetUsersToInvite(invitedUsers);
        var categories = await _categoriesService.GetAll();
        var user = await _usersService.GetByEmail(User.Identity.Name);

        ViewBag.Categories = categories;

        if (!String.IsNullOrEmpty(searchString))
        {
            usersToInvite = usersToInvite.Where(u => u.FullName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) || 
                u.EmailAddress.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        if (categoryId != null)
        {
            usersToInvite = usersToInvite.Where(u => u.Category.Id == categoryId.Value);
        }

        return View(usersToInvite);
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> InviteUser(int workspace, string id)
    {
        var workspaceObject = await _workspacesService.FindWorkspace(workspace);
        var user = await _usersService.GetByEmail(id);
        var initiator = await _usersService.GetByEmail(User.Identity.Name);
        await _workspacesService.AddUserToWorkspace(workspaceObject, user, initiator);

        return RedirectToAction("InviteUsers");
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> RemoveUsers(int workspace, int? categoryId, string searchString)
    {
        var invitedUsers = await _workspacesService.GetInvitedUsers(workspace);
        var categories = await _categoriesService.GetAll();
        var user = await _usersService.GetByEmail(User.Identity.Name);

        if (invitedUsers.Count() == 1) 
        {
            ViewBag.UsersCount = "error";
        }

        ViewBag.Categories = categories;

        if (!String.IsNullOrEmpty(searchString))
        {
            invitedUsers = invitedUsers.Where(u => u.FullName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
                u.EmailAddress.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        if (categoryId != null)
        {
            invitedUsers = invitedUsers.Where(u => u.Category.Id == categoryId.Value);
        }

        return View(invitedUsers);
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> RemoveUser(int workspace, string id)
    {
        var workspaceObject = await _workspacesService.FindWorkspace(workspace);
        var user = await _usersService.GetByEmail(id);
        var currentUser = await _usersService.GetByEmail(User.Identity.Name);
        if (currentUser != user && user.Category.CategoryName == "Manager") 
        {
            TempData["error"] = "Возникли проблемы при удалении пользователя. Возможно у вас недостаточно прав для этого действия";
        }
        else
        {
            await _workspacesService.RemoveUser(workspaceObject, user);
        }

        return RedirectToAction("RemoveUsers");
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var workspace = await _workspacesService.FindWorkspace(id);
        return View(workspace);
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var workspace = await _workspacesService.FindWorkspace(id);

        var workspaceTasks = await _tasksService.GetWorkspaceTasks(id);

        foreach (var task in workspaceTasks)
        {
            await _usersService.DeleteTaskFromUsers(task.Id);
        }

        var initiator = await _usersService.GetByEmail(User.Identity.Name);

        await _workspacesService.DeleteWorkspace(workspace, initiator);

        return RedirectToAction("Index");
    }
}
