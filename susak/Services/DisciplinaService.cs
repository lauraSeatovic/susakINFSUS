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
        return await _repository.GetAllAsync(search);
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
        return _repository.GetTreneriSelectList();
    }
}
