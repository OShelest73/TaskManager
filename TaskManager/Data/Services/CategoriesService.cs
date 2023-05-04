using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public class CategoriesService : ICategoriesService
{
    private readonly AppDBContext _dbContext;

    public CategoriesService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        var result = await _dbContext.Categories.ToListAsync();
        return result;
    }

    public async Task<CategoryModel> GetById(int id)
    {
        var result = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return result;
    }
}
