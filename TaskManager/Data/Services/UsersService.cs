﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Net.Mail;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class UsersService : IUsersService
{
    private readonly AppDBContext _dbContext;

    public UsersService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(UserModel user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAll()
    {
        var result = await _dbContext.Users.Include(c => c.Category).ToListAsync();
        return result;
    }

    public async Task<UserModel> GetByEmail(string emailAddress)
    {
        var result = await _dbContext.Users.Include(c => c.Category).FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        return result;
    }

    public async Task<UserModel> GetByEmailWithTask(string emailAddress)
    {
        var result = await _dbContext.Users.Include(c => c.Category).Include(u => u.Task).ThenInclude(t => t.Author).FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        return result;
    }

    public async Task<IEnumerable<WorkspaceModel>> GetUsersWorkspaces(string userId)
    {
        var user = await _dbContext.Users.Include(u => u.Workspaces).FirstOrDefaultAsync(u => u.EmailAddress == userId);

        return user.Workspaces;
    }

    public async Task<IEnumerable<UserModel>> GetUsersToAppoint(CategoryModel category, WorkspaceModel workspace)
    {
        var result = await _dbContext.Users.Where(u => u.Category == category && u.Workspaces.Contains(workspace) && u.TaskId == null).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<UserModel>> GetUsersToInvite(IEnumerable<UserModel> invitedUsers)
    {
        var users = await GetAll();
        users = users.Except(invitedUsers);
        return users;
    }

    public async Task AppointUser(UserModel user, int taskId)
    {
        user.TaskId = taskId;
        await Update(user);
    }

    public async Task DeleteUser(UserModel user)
    {
        if (_dbContext.Users.Where(u => u.Category.CategoryName == "Manager").Count() > 1)
        {
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteTaskFromUsers(int taskId)
    {
        var users = await _dbContext.Users.Where(u => u.TaskId == taskId).ToListAsync();
        foreach (var user in users)
        {
            user.TaskId = null;
            await Update(user);
        }
    }

    public bool NotUnique(string emailAddress)
    {
        var result = _dbContext.Users.Any(u => u.EmailAddress == emailAddress);
        return result;
    }

    public async Task<UserModel> LogIn(string emailAddress, string password)
    {
        var result = await _dbContext.Users.Include(c => c.Category).FirstOrDefaultAsync(u => u.EmailAddress == emailAddress && u.Password == password);
        return result;
    }

    public async Task Update(UserModel newUser)
    {
        _dbContext.Update(newUser);
        await _dbContext.SaveChangesAsync();
    }
}
