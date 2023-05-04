using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class WorkspaceService : IWorkspacesService
{
    private readonly AppDBContext _dbContext;

    public WorkspaceService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WorkspaceModel>> GetAllWorkspaces()
    {
        var result = await _dbContext.Workspaces.ToListAsync();
        return result;
    }

    public async Task<WorkspaceModel> FindWorkspace(int workspaceId)
    {
        var result = await _dbContext.Workspaces.Include(w => w.Users).FirstOrDefaultAsync(w => w.Id == workspaceId);
        return result;
    }

    public async Task<IEnumerable<UserModel>> GetInvitedUsers(int workspaceId)
    {
        var workspace = await FindWorkspace(workspaceId);
        var invitedUsers = workspace.Users;

        return invitedUsers;
    }

    public async Task Add(WorkspaceModel workspace, UserModel user)
    {
        await _dbContext.Workspaces.AddAsync(workspace);
        await _dbContext.SaveChangesAsync();

        await AddUserToWorkspace(workspace, user);
    }

    public async Task AddUserToWorkspace(WorkspaceModel workspace, UserModel user)
    {
        workspace.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveUser(WorkspaceModel workspace, UserModel user)
    {
        workspace.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}
