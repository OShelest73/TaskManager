using TaskManager.Models;

namespace TaskManager.Data.Services;

public interface IStatusesService
{
    Task<IEnumerable<StatusModel>> GetAll();
    Task<StatusModel> GetById(int id);
}
