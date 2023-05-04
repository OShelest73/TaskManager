﻿using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface IUsersService
{
    Task<IEnumerable<UserModel>> GetAll();
    Task<UserModel> GetByEmail(string emailAddress);
    Task Add(UserModel user);
    Task Update(UserModel newUser);
    void Delete(int id);
    Task<UserModel> LogIn(string emailAddress, string password);
    Task<IEnumerable<UserModel>> GetAppointedUsers(int taskId);
    Task<IEnumerable<UserModel>> GetUsersToAppoint(CategoryModel category);
    Task AppointUser(UserModel user, int taskId);
    Task DeleteTaskFromUser(int taskId);
    Task<IEnumerable<UserModel>> GetUsersToInvite(IEnumerable<UserModel> invitedUsers);
    Task<IEnumerable<WorkspaceModel>> GetUsersWorkspaces(string userId);
    Task DeleteUser(UserModel user);
}
