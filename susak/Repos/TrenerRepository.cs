using Microsoft.EntityFrameworkCore;
using susak.Models;

public class TrenerRepository : ITrenerRepository
{
    private readonly susakContext _context;

    public TrenerRepository(susakContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trener>> GetAllAsync()
    {
        return await _context.Trener.Include(t => t.Korisnik).ToListAsync();
    }

    public async Task<Trener?> GetByIdAsync(int id)
    {
        return await _context.Trener.Include(t => t.Korisnik).FirstOrDefaultAsync(t => t.TrenerId == id);
    }

    public async Task AddAsync(Trener trener)
    {
        _context.Trener.Add(trener);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trener trener)
    {
        _context.Update(trener);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var trener = await _context.Trener.Include(d => d.Disciplina)
            .FirstOrDefaultAsync(d => d.TrenerId == id);
        if (trener != null)
        {
            trener.Disciplina.Clear();
            _context.Trener.Remove(trener);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Trener.AnyAsync(t => t.TrenerId == id);
    }
}
