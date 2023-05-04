﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Data;
using TaskManager.Data.Services;
using TaskManager.Models;
using TaskManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Controllers;
public class UsersController : Controller
{
    private readonly IUsersService _usersService;
    private readonly ICategoriesService _categoriesService;

    public UsersController(IUsersService usersService, ICategoriesService categoriesService)
    {
        _usersService = usersService;
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        if (ModelState.IsValid)
        {
            UserModel user = await _usersService.GetByEmail(model.EmailAddress);
            if (user != null && AssemblePassword(model.Password, user.Salt) == user.Password)
            {
                await Authenticate(model.EmailAddress, user.Category.CategoryName);

                return RedirectToAction("Index", "Workspaces");
            }
            ModelState.AddModelError("", "Wrong email or password");
        }
        return View(model);
    }

    public async Task<IActionResult> Index()
    {
        var allUsers = await _usersService.GetAll();
        return View(allUsers);
    }

    [HttpGet]
    public async Task<IActionResult> Register() 
    {
        var categories = await _categoriesService.GetAll();
        SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
        ViewBag.Categories = selectCategories;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel user)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoriesService.GetAll();
            SelectList selectCategories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Categories = selectCategories;
            return View();
        }
        UserModel passedUser = new UserModel();
        passedUser.FullName = user.FullName;
        passedUser.EmailAddress = user.EmailAddress;
        passedUser.Category = await _categoriesService.GetById(user.CategoryId.Value);

        byte[] salt = GenerateSalt();
        passedUser.Password = AssemblePassword(user.Password, salt);
        passedUser.Salt = Convert.ToBase64String(salt);

        await _usersService.Add(passedUser);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(string id)
    {
        var userDetails = await _usersService.GetByEmail(id);

        if (userDetails == null)
        {
            return View("NotFound");
        }

        return View(userDetails);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var userDetails = await _usersService.GetByEmail(id);

        if (userDetails == null)
        {
            return View("NotFound");
        }

        return View(userDetails);
    }

    public async Task<IActionResult> DeleteConfirm(string id)
    {
        var user = await _usersService.GetByEmail(id);
        return View(user);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetByEmail(id);

        await _usersService.DeleteUser(user);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind($"Id, EmailAddress, FullName, Category")] UserModel user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        await _usersService.Update(user);
        return RedirectToAction(nameof(Index));
    }

    private async Task Authenticate(string emailAddress, string category)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, emailAddress),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, category)
            };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("LogIn");
    }

    private string AssemblePassword(string password, byte[] saltBytes)
    {
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 1000);
        var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(128));

        return hashPassword;
    }

    private string AssemblePassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 1000);
        return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(128));
    }

    private byte[] GenerateSalt()
    {
        int minSaltSize = 4;
        int maxSaltSize = 8;

        Random random = new Random();
        int size = random.Next(minSaltSize, maxSaltSize);
        var saltBytes = new byte[size];

        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetNonZeroBytes(saltBytes);

        return saltBytes;
    }
}