using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class WorkspaceService : IWorkspacesService
{
    private readonly AppDBContext _dbContext;
    private readonly INotificationsService _notificationsService;

    public WorkspaceService(AppDBContext dbContext, INotificationsService notificationsService)
    {
        _dbContext = dbContext;
        _notificationsService = notificationsService;
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

    public async Task Add(WorkspaceModel workspace, UserModel user, UserModel initiator)
    {
        await _dbContext.Workspaces.AddAsync(workspace);
        await _dbContext.SaveChangesAsync();

        await AddUserToWorkspace(workspace, user, initiator);
    }

    public async Task AddUserToWorkspace(WorkspaceModel workspace, UserModel user, UserModel initiator)
    {
        workspace.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        await _notificationsService.CreateNotification(initiator.EmailAddress, user,
            $"Вы были приглашены в рабочее пространство {workspace.WorkspaceName}",
            $"Вы были приглашены в рабочее пространство {workspace.WorkspaceName} пользователем {initiator.FullName} (Email для связи - {initiator.EmailAddress})");
    }

    public async Task RemoveUser(WorkspaceModel workspace, UserModel user)
    {
        if (user.Task != null)
        {
            user.Task = null;
            await _dbContext.SaveChangesAsync();
        }
        workspace.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteWorkspace(WorkspaceModel workspace, UserModel initiator)
    {
        workspace.Users.Remove(initiator);
        await _notificationsService.CreateNotification(initiator.EmailAddress, workspace.Users,
            $"Удаление рабочего пространства {workspace.WorkspaceName}",
            $"Рабочее пространство {workspace.WorkspaceName} было удалено пользователем {initiator.FullName} (Email для связи - {initiator.EmailAddress}). Это привело к удалению связанных с ним заданий");
        _dbContext.Remove(workspace);
        await _dbContext.SaveChangesAsync();
    }
}
