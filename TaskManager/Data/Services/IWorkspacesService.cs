using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface IWorkspacesService
{
    Task Add(WorkspaceModel workspace, UserModel user);
    Task AddUserToWorkspace(WorkspaceModel workspace, UserModel user);
    Task DeleteWorkspace(WorkspaceModel workspace);
    Task<WorkspaceModel> FindWorkspace(int workspaceId);
    Task<IEnumerable<WorkspaceModel>> GetAllWorkspaces();
    Task<IEnumerable<UserModel>> GetInvitedUsers(int workspaceId);
    Task RemoveUser(WorkspaceModel workspace, UserModel user);
}
