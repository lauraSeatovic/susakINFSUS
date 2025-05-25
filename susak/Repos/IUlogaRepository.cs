using susak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUlogaRepository
{
    Task<IEnumerable<Uloga>> GetAllAsync();
    Task<Uloga?> GetByIdAsync(int id);
    Task AddAsync(Uloga uloga);
    Task UpdateAsync(Uloga uloga);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
