using susak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITrenerRepository
{
    Task<IEnumerable<Trener>> GetAllAsync();
    Task<Trener?> GetByIdAsync(int id);
    Task AddAsync(Trener trener);
    Task UpdateAsync(Trener trener);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

}
