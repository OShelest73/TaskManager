using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskManager.Data;
using TaskManager.Data.Services;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;

[Authorize]
public class TasksController : Controller
{
    private readonly ITasksService _tasksService;
    private readonly IUsersService _usersService;
    private readonly ICategoriesService _categoriesService;
    private readonly IStatusesService _statusesService;
    private readonly IWorkspacesService _workspacesService;

    public TasksController(ITasksService tasksService, IUsersService usersService, ICategoriesService categoriesService, IStatusesService statusesService, IWorkspacesService workspacesService)
    {
        _tasksService = tasksService;
        _usersService = usersService;
        _categoriesService = categoriesService;
        _statusesService = statusesService;
        _workspacesService = workspacesService;
    }

    public async Task<IActionResult> Index(int workspace, int? categoryId, int? statusId, string searchString)
    {
        var workspaceTasks = await _tasksService.GetWorkspaceTasks(workspace);
        var categories = await _categoriesService.GetAll();
        var statuses = await _statusesService.GetAll();

        ViewBag.Categories = categories;
        ViewBag.Statuses = statuses;

        if (!String.IsNullOrEmpty(searchString))
        {
            workspaceTasks = workspaceTasks.Where(t => t.TaskName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
            t.Description.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        if (categoryId != null)
        {
            workspaceTasks = workspaceTasks.Where(w => w.Category.Id == categoryId.Value);
            TempData["sortCategory"] = categoryId.Value;
        }
        if (statusId != null)
        {
            workspaceTasks = workspaceTasks.Where(w => w.Status.Id == statusId.Value);
            TempData["sortStatus"] = statusId.Value;
        }

        ViewBag.Workspace = workspace;
        return View(workspaceTasks);
    }

    public async Task<IActionResult> Details(int workspace, int id)
    {
        ViewBag.Workspace = workspace;
        var task = await _tasksService.FindTaskWithUsers(id);
        ViewBag.AppointedUsers = task.AppointedUsers;
        return View(task);
    }

    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> Create(int workspace)
    {
        var categories = await _categoriesService.GetAll();
        SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
        ViewBag.Categories = selectCategories;
        var statuses = await _statusesService.GetAll();
        SelectList selectStatuses = new SelectList(statuses, "Id", "StatusName");
        ViewBag.Statuses = selectStatuses;
        return View();
    }
    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskViewModel task)
    {
        TaskModel createdTask = await ConvertFromViewModel(task);
        TimeSpan difference = TimeSpan.FromMinutes(5);

        if (createdTask.FinishDate.Subtract(createdTask.CreatedDate) < difference)
        {
            ModelState.AddModelError(string.Empty, 
                "Промежуток между датой и временем создания и датой и временем завершения слишком мал (должен быть не менее 5 минут). Пожалуйста, задайте корректные дату и время завершения задания");
        }

        if (!ModelState.IsValid)
        {
            var categories = await _categoriesService.GetAll();
            SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Categories = selectCategories;
            var statuses = await _statusesService.GetAll();
            SelectList selectStatuses = new SelectList(statuses, "Id", "StatusName");
            ViewBag.Statuses = selectStatuses;
            return View();
        }

        createdTask.Category = await _categoriesService.GetById(task.Category);
        createdTask.Status = await _statusesService.GetById(task.Status);

        int workspaceId = Convert.ToInt32(HttpContext.GetRouteValue("workspace"));
        createdTask.Workspace = await _workspacesService.FindWorkspace(workspaceId);

        createdTask.Author = await _usersService.GetByEmail(User.Identity.Name);

        await _tasksService.Add(createdTask);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete()
    {
        int taskId = Convert.ToInt32(HttpContext.GetRouteValue("Id"));

        var initiator = await _usersService.GetByEmail(User.Identity.Name);

        await _tasksService.Delete(taskId, initiator);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _tasksService.FindTask(id.Value);

        if (task == null)
        {
            return NotFound();
        }

        var categories = await _categoriesService.GetAll();
        var statuses = await _statusesService.GetAll();

        if (categories != null && statuses != null)
        {
            SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Categories = selectCategories;

            SelectList selectStatuses = new SelectList(statuses, "Id", "StatusName");
            ViewBag.Statuses = selectStatuses;

            TaskViewModel viewTask = ConvertToViewModel(task);

            return View(viewTask);
        }
        else
        {
            return NotFound();
        }        
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, TaskViewModel task)
    {
        var correctTask = await _tasksService.FindTask(id.Value);

        TimeSpan difference = TimeSpan.FromMinutes(5);

        if (task.FinishDate.Subtract(correctTask.CreatedDate.ToLocalTime()) < difference)
        {
            ModelState.AddModelError(string.Empty,
                "Промежуток между датой и временем создания и датой и временем завершения слишком мал (должен быть не менее 5 минут). Пожалуйста, задайте корректные дату и время завершения задания");
        }

        if (id != task.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var categories = await _categoriesService.GetAll();
            SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Categories = selectCategories;
            var statuses = await _statusesService.GetAll();
            SelectList selectStatuses = new SelectList(statuses, "Id", "StatusName");
            ViewBag.Statuses = selectStatuses;            

            TaskViewModel viewTask = ConvertToViewModel(correctTask);

            return View(viewTask);
        }

        await ConvertFromViewModel(task, correctTask);

        var initiator = await _usersService.GetByEmail(User.Identity.Name);

        await _tasksService.Update(correctTask, initiator);
        return RedirectToAction("Details", new{id = id});
    }

    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> AppointUsers(int id)
    {
        var task = await _tasksService.FindTaskWithUsers(id);
        AppointViewModel usersList = new();
        usersList.usersList = await _usersService.GetUsersToAppoint(task.Category, task.Workspace);
        IEnumerable<UserModel> appointedUsers = task.AppointedUsers;
        if (appointedUsers == null || appointedUsers.Count() == 0)
        {
            usersList.allUsers = usersList.usersList;
        }
        else if(usersList.usersList == null)
        {
            usersList.allUsers = appointedUsers;
        }
        else
        {
            usersList.allUsers = usersList.usersList.Concat(appointedUsers);
        }
        ViewBag.UsersId = new MultiSelectList(usersList.allUsers, "EmailAddress", "FullName", appointedUsers.Select(au => au.EmailAddress));
        
        return View(usersList);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AppointUsers(int id, IEnumerable<string> selectList)
    {
        List<UserModel> usersList = new();

        foreach (var userId in selectList)
        {
            var user = await _usersService.GetByEmail(userId.ToString());
            usersList.Add(user);
        }

        var initiator = await _usersService.GetByEmail(User.Identity.Name);
        await _tasksService.AppointUsers(id, usersList, initiator);

        return RedirectToAction("Details", new { id = id });
    }

    private async Task<TaskModel> ConvertFromViewModel(TaskViewModel taskViewModel)
    {
        TaskModel task = new TaskModel();

        task.TaskName = taskViewModel.TaskName;
        task.Description = taskViewModel.Description;
        task.Notes = taskViewModel.Notes;
        task.FinishDate = taskViewModel.FinishDate.ToUniversalTime();
        task.Category = await _categoriesService.GetById(taskViewModel.Category);
        task.Status = await _statusesService.GetById(taskViewModel.Status);

        return task;
    }

    private async Task ConvertFromViewModel(TaskViewModel taskViewModel, TaskModel task)
    {
        task.TaskName = taskViewModel.TaskName;
        task.Description = taskViewModel.Description;
        task.Notes = taskViewModel.Notes;
        task.FinishDate = taskViewModel.FinishDate.ToUniversalTime();
        task.Category = await _categoriesService.GetById(taskViewModel.Category);
        task.Status = await _statusesService.GetById(taskViewModel.Status);
    }

    private TaskViewModel ConvertToViewModel(TaskModel taskModel)
    {
        TaskViewModel viewTask = new TaskViewModel();

        viewTask.TaskName = taskModel.TaskName;
        viewTask.Description = taskModel.Description;
        viewTask.Notes = taskModel.Notes;
        viewTask.FinishDate = taskModel.FinishDate.ToLocalTime();
        viewTask.Category = taskModel.Category.Id;
        viewTask.Status = taskModel.Status.Id;

        return viewTask;
    }
}
