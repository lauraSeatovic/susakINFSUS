using Microsoft.EntityFrameworkCore;
using susak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UlogaRepository : IUlogaRepository
{
    private readonly susakContext _context;

    public UlogaRepository(susakContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Uloga>> GetAllAsync()
    {
        return await _context.Uloga.ToListAsync();
    }

    public async Task<Uloga?> GetByIdAsync(int id)
    {
        return await _context.Uloga.FindAsync(id);
    }

    public async Task AddAsync(Uloga uloga)
    {
        _context.Uloga.Add(uloga);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Uloga uloga)
    {
        _context.Entry(uloga).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var uloga = await _context.Uloga.FindAsync(id);
        if (uloga != null)
        {
            _context.Uloga.Remove(uloga);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Uloga.AnyAsync(e => e.UlogaId == id);
    }
}
