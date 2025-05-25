using susak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UlogaService : IUlogaService
{
    private readonly IUlogaRepository _ulogaRepository;

    public UlogaService(IUlogaRepository ulogaRepository)
    {
        _ulogaRepository = ulogaRepository;
    }

    public async Task<IEnumerable<Uloga>> GetAllAsync()
    {
        return await _ulogaRepository.GetAllAsync();
    }

    public async Task<Uloga?> GetByIdAsync(int id)
    {
        return await _ulogaRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Uloga uloga)
    {
        await _ulogaRepository.AddAsync(uloga);
    }

    public async Task UpdateAsync(Uloga uloga)
    {
        await _ulogaRepository.UpdateAsync(uloga);
    }

    public async Task DeleteAsync(int id)
    {
        await _ulogaRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _ulogaRepository.ExistsAsync(id);
    }
}
