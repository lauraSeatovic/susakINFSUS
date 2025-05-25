using susak.Models;

public interface IDisciplinaService
{
    Task<List<Disciplina>> GetAllAsync(string? search = null);
    Task<Disciplina?> GetByIdAsync(int id);
    Task<Disciplina?> GetByIdWithRelationsAsync(int id);
    Task AddAsync(Disciplina disciplina);
    Task UpdateAsync(Disciplina disciplina);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveAsync();
    Task<Disciplina?> GetDisciplinaForEditAsync(int id);
    List<object> GetTreneriSelectList();
    Task EditMasterDetailAsync(Disciplina disciplina, int? DodajTrenerId, int[]? TreneriZaBrisanje, List<int>? DodajTreneriId);
    void AddClanToDisciplina(int disciplinaId, Clan clan);
    void AddPostojeciClan(int disciplinaId, int clanId);
    Task RemoveClanAsync(int disciplinaId, int clanId);
    List<object> GetClanoviSelectList();
    Task CreateDisciplina(Disciplina disciplina, List<int> trenerIds);


}
