using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryModel>> GetAll();
    Task<CategoryModel> GetById(int id);
}
