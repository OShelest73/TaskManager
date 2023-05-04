﻿using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskManager.Data;
using TaskManager.Data.Services;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;
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
        var user = await _usersService.GetByEmail(User.Identity.Name);

        ViewBag.Role = user.Category.CategoryName;
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
        var user = await _usersService.GetByEmail(User.Identity.Name);
        ViewBag.Workspace = workspace;
        ViewBag.Role = user.Category.CategoryName;
        var task = await _tasksService.FindTask(id);
        var appointedUsers = await _usersService.GetAppointedUsers(task.Id);
        ViewBag.AppointedUsers = appointedUsers;
        return View(task);
    }

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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskViewModel task)
    {
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
        
        TaskModel createdTask = ConvertFromViewModel(task);

        createdTask.Category = await _categoriesService.GetById(task.Category);
        createdTask.Status = await _statusesService.GetById(task.Status);

        int workspaceId = Convert.ToInt32(HttpContext.GetRouteValue("workspace"));
        createdTask.Workspace = await _workspacesService.FindWorkspace(workspaceId);

        createdTask.Author = await _usersService.GetByEmail(User.Identity.Name);

        await _tasksService.Add(createdTask);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete()
    {
        int taskId = Convert.ToInt32(HttpContext.GetRouteValue("Id"));

        await _usersService.DeleteTaskFromUser(taskId);
        await _tasksService.Delete(taskId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categories = await _categoriesService.GetAll();
        SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
        ViewBag.Categories = selectCategories;
        var statuses = await _statusesService.GetAll();
        SelectList selectStatuses = new SelectList(statuses, "Id", "StatusName");
        ViewBag.Statuses = selectStatuses;

        var task = await _tasksService.FindTask(id.Value);

        TaskViewModel viewTask = ConvertToViewModel(task);

        return View(viewTask);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, TaskViewModel task)
    {
        if (id != task.Id)
        {
            return NotFound();
        }

        var correctTask = await _tasksService.FindTask(id.Value);

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

        ConvertFromViewModel(task, correctTask);

        await _tasksService.Update(correctTask);
        return RedirectToAction("Details", new{id = id});
    }

    public async Task<IActionResult> AppointUsers(int id)
    {
        var task = await _tasksService.FindTask(id);
        AppointViewModel usersList = new();
        usersList.usersList = await _usersService.GetUsersToAppoint(task.Category);
        IEnumerable<UserModel> appointedUsers = await _usersService.GetAppointedUsers(task.Id);
        if (appointedUsers == null)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AppointUsers(int id, IEnumerable<string> selectList)
    {
        await _usersService.DeleteTaskFromUser(id);
        foreach (var userId in selectList)
        {
            var user = await _usersService.GetByEmail(userId.ToString());
            await _usersService.AppointUser(user, id);
        }

        return RedirectToAction("Details", new { id = id });
    }

    private TaskModel ConvertFromViewModel(TaskViewModel taskViewModel)
    {
        TaskModel task = new TaskModel();

        task.TaskName = taskViewModel.TaskName;
        task.Description = taskViewModel.Description;
        task.Notes = taskViewModel.Notes;
        task.FinishDate = taskViewModel.FinishDate.ToUniversalTime();

        return task;
    }

    private void ConvertFromViewModel(TaskViewModel taskViewModel, TaskModel task)
    {
        task.TaskName = taskViewModel.TaskName;
        task.Description = taskViewModel.Description;
        task.Notes = taskViewModel.Notes;
        task.FinishDate = taskViewModel.FinishDate.ToUniversalTime();
    }

    private TaskViewModel ConvertToViewModel(TaskModel taskModel)
    {
        TaskViewModel viewTask = new TaskViewModel();

        viewTask.TaskName = taskModel.TaskName;
        viewTask.Description = taskModel.Description;
        viewTask.Notes = taskModel.Notes;
        viewTask.FinishDate = taskModel.FinishDate;
        viewTask.Category = taskModel.Category.Id;
        viewTask.Status = taskModel.Status.Id;

        return viewTask;
    }
}