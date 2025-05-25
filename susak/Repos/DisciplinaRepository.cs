using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using susak.Models;

public class DisciplinaRepository : IDisciplinaRepository
{
    private readonly susakContext _context;

    public DisciplinaRepository(susakContext context)
    {
        _context = context;
    }

    public async Task<List<Disciplina>> GetAllAsync()
    {
        return await _context.Disciplina.ToListAsync();
    }

    public async Task<Disciplina?> GetByIdAsync(int id)
    {
        return await _context.Disciplina.FirstOrDefaultAsync(m => m.DisciplinaId == id);
    }

    public async Task<Disciplina?> GetByIdWithRelationsAsync(int id)
    {
        return await _context.Disciplina
            .Include(d => d.Trener)
            .Include(d => d.Clan)
            .Include(d => d.Trening)
            .FirstOrDefaultAsync(m => m.DisciplinaId == id);
    }

    public async Task AddAsync(Disciplina disciplina)
    {
        _context.Add(disciplina);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Disciplina disciplina)
    {
        _context.Update(disciplina);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var disciplina = await _context.Disciplina
            .Include(d => d.Trener) 
            .Include(d => d.Clan)
            .Include(d => d.Trening)
            .FirstOrDefaultAsync(d => d.DisciplinaId == id);

        if (disciplina == null)
            throw new InvalidOperationException("Disciplina ne postoji.");

        disciplina.Trening.Clear();
        disciplina.Trener.Clear();
        disciplina.Clan.Clear();

        // Na kraju brišemo disciplinu
        _context.Disciplina.Remove(disciplina);

        await _context.SaveChangesAsync();
    }


    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Disciplina.AnyAsync(e => e.DisciplinaId == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Disciplina?> GetDisciplinaForEditAsync(int id)
    {
        return await _context.Disciplina.Include(d => d.Trener).FirstOrDefaultAsync(d => d.DisciplinaId == id);
    }

    public List<object> GetTreneriSelectList()
    {
        return _context.Trener.Select(t => new {
            t.TrenerId,
            ImePrezime = t.Ime + " " + t.Prezime
        }).ToList<object>();
    }

    public List<object> GetClanoviSelectList()
    {
        return _context.Clan.Select(t => new {
            t.ClanId,
            ImePrezime = t.Ime + " " + t.Prezime
        }).ToList<object>();
    }

    public async Task EditMasterDetailAsync(Disciplina disciplina, int? DodajTrenerId, int[]? TreneriZaBrisanje, List<int>? DodajTreneriId)
    {
        var disciplinaDb = await _context.Disciplina.Include(d => d.Trener).FirstOrDefaultAsync(d => d.DisciplinaId == disciplina.DisciplinaId);
        if (disciplinaDb == null) return;

        disciplinaDb.Naziv = disciplina.Naziv;
        disciplinaDb.Opis = disciplina.Opis;

        if (TreneriZaBrisanje != null)
        {
            foreach (var trenerId in TreneriZaBrisanje)
            {
                var trenerZaUkloniti = disciplinaDb.Trener.FirstOrDefault(t => t.TrenerId == trenerId);
                if (trenerZaUkloniti != null)
                {
                    disciplinaDb.Trener.Remove(trenerZaUkloniti);
                }
            }
        }

        if (DodajTrenerId.HasValue && !disciplinaDb.Trener.Any(t => t.TrenerId == DodajTrenerId.Value))
        {
            var noviTrener = await _context.Trener.FindAsync(DodajTrenerId.Value);
            if (noviTrener != null)
            {
                disciplinaDb.Trener.Add(noviTrener);
            }
        }

        if (DodajTreneriId != null)
        {
            foreach (var trenerId in DodajTreneriId)
            {
                if (!disciplinaDb.Trener.Any(t => t.TrenerId == trenerId))
                {
                    var trenerZaDodati = await _context.Trener.FindAsync(trenerId);
                    if (trenerZaDodati != null)
                    {
                        disciplinaDb.Trener.Add(trenerZaDodati);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public void AddClanToDisciplina(int disciplinaId, Clan clan)
    {
        _context.Clan.Add(clan);
        _context.SaveChanges();

        var disciplina = _context.Disciplina.Include(d => d.Clan)
                            .FirstOrDefault(d => d.DisciplinaId == disciplinaId);

        if (disciplina != null)
        {
            disciplina.Clan.Add(clan);
            _context.SaveChanges();
        }
    }

    public void AddPostojeciClan(int disciplinaId, int clanId)
    {
        var disciplina = _context.Disciplina.Include(d => d.Clan)
                            .FirstOrDefault(d => d.DisciplinaId == disciplinaId);
        var clan = _context.Clan.Find(clanId);

        if (disciplina != null && clan != null && !disciplina.Clan.Contains(clan))
        {
            disciplina.Clan.Add(clan);
            _context.SaveChanges();
        }
    }

    public async Task RemoveClanAsync(int disciplinaId, int clanId)
    {
        var disciplina = await _context.Disciplina
            .Include(d => d.Clan)
            .FirstOrDefaultAsync(d => d.DisciplinaId == disciplinaId);

        if (disciplina == null) return;

        var clanToRemove = disciplina.Clan.FirstOrDefault(c => c.ClanId == clanId);
        if (clanToRemove != null)
        {
            disciplina.Clan.Remove(clanToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public void CreateDisciplina(Disciplina disciplina, List<int> trenerIds)
    {
        if (trenerIds != null && trenerIds.Any())
        {
            disciplina.Trener = _context.Trener
                .Where(t => trenerIds.Contains(t.TrenerId))
                .ToList();
        }

        _context.Disciplina.Add(disciplina);
        _context.SaveChanges();
    }
    public async Task<List<Trening>> GetTreninziForDisciplinaAsync(int disciplinaId)
    {
        return await _context.Trening
            .Where(t => t.DisciplinaId == disciplinaId)
            .ToListAsync();
    }

    public async Task<List<Trening>> GetTreninziForTrenerAsync(int trenerId)
    {
        return await _context.Trening
            .Where(t => t.Disciplina.Trener.Any(tr => tr.TrenerId == trenerId))
            .ToListAsync();
    }


}
