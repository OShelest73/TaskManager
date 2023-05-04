using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class StatusesService : IStatusesService
{
    private readonly AppDBContext _dbContext;

    public StatusesService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<StatusModel>> GetAll()
    {
        var result = await _dbContext.Statuses.ToListAsync();
        return result;
    }

    public async Task<StatusModel> GetById(int id)
    {
        var result = await _dbContext.Statuses.FirstOrDefaultAsync(c => c.Id == id);
        return result;
    }
}
