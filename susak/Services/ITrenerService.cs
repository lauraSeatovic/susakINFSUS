using susak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITrenerService
{
    Task<IEnumerable<Trener>> GetAllAsync(string? search = null);
    Task<Trener?> GetByIdAsync(int id);
    Task AddAsync(Trener trener);
    Task UpdateAsync(Trener trener);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
