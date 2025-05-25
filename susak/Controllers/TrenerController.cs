using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using susak.Models;

public class TrenerController : Controller
{
    private readonly ITrenerService _service;
    private readonly susakContext _context;

    public TrenerController(ITrenerService service, susakContext context)
    {
        _service = service;
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        var treneri = await _service.GetAllAsync(searchString);
        return View(treneri);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var trener = await _service.GetByIdAsync(id.Value);
        return trener == null ? NotFound() : View(trener);
    }

    public IActionResult Create()
    {
        ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TrenerId,Ime,Prezime,StrucnaSprema,KorisnikId")] Trener trener)
    {
        if (ModelState.IsValid)
        {
            await _service.AddAsync(trener);
            return RedirectToAction(nameof(Index));
        }
        ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
        return View(trener);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var trener = await _service.GetByIdAsync(id.Value);
        if (trener == null) return NotFound();

        ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
        return View(trener);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TrenerId,Ime,Prezime,StrucnaSprema,KorisnikId")] Trener trener)
    {
        if (id != trener.TrenerId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _service.UpdateAsync(trener);
            }
            catch
            {
                if (!await _service.ExistsAsync(trener.TrenerId)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
        return View(trener);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var trener = await _service.GetByIdAsync(id.Value);
        return trener == null ? NotFound() : View(trener);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
