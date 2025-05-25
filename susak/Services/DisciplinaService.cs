using susak.Models;

public class DisciplinaService : IDisciplinaService
{
    private readonly IDisciplinaRepository _repository;

    public DisciplinaService(IDisciplinaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Disciplina>> GetAllAsync(string? search = null)
    {
        var all = await _repository.GetAllAsync();

        if (!string.IsNullOrEmpty(search))
            all = all.Where(d => d.Naziv.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        return all;
    }

    public async Task<Disciplina?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Disciplina?> GetByIdWithRelationsAsync(int id)
    {
        return await _repository.GetByIdWithRelationsAsync(id);
    }

    public async Task AddAsync(Disciplina disciplina)
    {
        await _repository.AddAsync(disciplina);
    }

    public async Task UpdateAsync(Disciplina disciplina)
    {
        await _repository.UpdateAsync(disciplina);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }

    public async Task SaveAsync()
    {
        await _repository.SaveAsync();
    }

    public async Task<Disciplina?> GetDisciplinaForEditAsync(int id)
    {
        return await _repository.GetDisciplinaForEditAsync(id);
    }

    public List<object> GetTreneriSelectList()
    {
        return _repository.GetTreneriSelectList();
    }

    public async Task EditMasterDetailAsync(Disciplina disciplina, int? DodajTrenerId, int[]? TreneriZaBrisanje, List<int>? DodajTreneriId)
    {
        if (DodajTrenerId.HasValue)
        {
            if (DodajTreneriId == null)
                DodajTreneriId = new List<int>();

            if (!DodajTreneriId.Contains(DodajTrenerId.Value))
                DodajTreneriId.Add(DodajTrenerId.Value);
        }
        var disciplinaTreninzi = await _repository.GetTreninziForDisciplinaAsync(disciplina.DisciplinaId);

        if (DodajTreneriId != null && disciplinaTreninzi.Any())
        {
            foreach (var trenerId in DodajTreneriId)
            {
                var trenerTreninzi = await _repository.GetTreninziForTrenerAsync(trenerId);

                foreach (var treningDisciplina in disciplinaTreninzi)
                {
                    var start1 = treningDisciplina.Datum;
                    var end1 = start1.Value.AddMinutes(treningDisciplina.Trajanje);

                    foreach (var treningTrenera in trenerTreninzi)
                    {
                        var start2 = treningTrenera.Datum;
                        var end2 = start2.Value.AddMinutes(treningTrenera.Trajanje);

                        bool overlaps = start1 < end2 && start2 < end1;

                        if (overlaps)
                        {
                            throw new InvalidOperationException(
                                $"Postoji preklapanje treninga.");
                        }
                    }
                }
            }
        }

        await _repository.EditMasterDetailAsync(disciplina, DodajTrenerId, TreneriZaBrisanje, DodajTreneriId);
    }

    public void AddClanToDisciplina(int disciplinaId, Clan clan)
    {
        _repository.AddClanToDisciplina(disciplinaId, clan);
    }

    public void AddPostojeciClan(int disciplinaId, int clanId)
    {
        _repository.AddPostojeciClan(disciplinaId, clanId);
    }

    public async Task RemoveClanAsync(int disciplinaId, int clanId)
    {
        await _repository.RemoveClanAsync(disciplinaId, clanId);
    }

    public List<object> GetClanoviSelectList()
    {
        return _repository.GetClanoviSelectList();
    }

    public async Task CreateDisciplina(Disciplina disciplina, List<int> trenerIds)
    {
        var disciplinaTreninzi = await _repository.GetTreninziForDisciplinaAsync(disciplina.DisciplinaId);

        if (trenerIds != null && disciplinaTreninzi.Any())
        {
            foreach (var trenerId in trenerIds)
            {
                var trenerTreninzi = await _repository.GetTreninziForTrenerAsync(trenerId);

                foreach (var treningDisciplina in disciplinaTreninzi)
                {
                    var start1 = treningDisciplina.Datum;
                    var end1 = start1.Value.AddMinutes(treningDisciplina.Trajanje);

                    foreach (var treningTrenera in trenerTreninzi)
                    {
                        var start2 = treningTrenera.Datum;
                        var end2 = start2.Value.AddMinutes(treningTrenera.Trajanje);

                        bool overlaps = start1 < end2 && start2 < end1;

                        if (overlaps)
                        {
                            throw new InvalidOperationException(
                                $"Postoji preklapanje treninga.");
                        }
                    }
                }
            }
        }

        _repository.CreateDisciplina(disciplina, trenerIds);
    }

}
