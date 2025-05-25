using System.Collections.Generic;
using System.Threading.Tasks;
using susak.Models;

public class TrenerService : ITrenerService
{
    private readonly ITrenerRepository _repository;

    public TrenerService(ITrenerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Trener>> GetAllAsync(string? search = null)
    {
        var all = await _repository.GetAllAsync();

        if (!string.IsNullOrEmpty(search))
            all = all.Where(d => d.Ime.Contains(search, StringComparison.OrdinalIgnoreCase) || d.Prezime.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        return all;
    }

    public async Task<Trener?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Trener trener)
    {
        await _repository.AddAsync(trener);
    }

    public async Task UpdateAsync(Trener trener)
    {
        await _repository.UpdateAsync(trener);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}
