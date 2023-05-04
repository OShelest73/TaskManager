using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class TasksService : ITasksService
{
    private readonly AppDBContext _dbContext;

    public TasksService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaskModel>> GetWorkspaceTasks(int workspaceId)
    {
        var result = await _dbContext.Tasks.Where(t => t.Workspace.Id == workspaceId).ToListAsync();
        return result;
    }

    public async Task Add(TaskModel task)
    {
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TaskModel> FindTask(int id)
    {
        var result = await _dbContext.Tasks.Include(t => t.Category).Include(t => t.Status).Include(t => t.Author).FirstOrDefaultAsync(t => t.Id == id);
        return result;
    }

    public async Task Update(TaskModel newTask)
    {
        _dbContext.Update(newTask);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        _dbContext.Remove(await FindTask(id));
        _dbContext.SaveChanges();
    }
}
