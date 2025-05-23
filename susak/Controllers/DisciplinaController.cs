using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using susak.Models;

namespace susak.Controllers
{
    public class DisciplinaController : Controller
    {
        private readonly susakContext _context;

        public DisciplinaController(susakContext context)
        {
            _context = context;
        }

        // GET: Disciplina
        /*
        public async Task<IActionResult> Index()
        {
            return View(await _context.Disciplina.ToListAsync());
        }
        */

        // GET: Disciplina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplina
                .FirstOrDefaultAsync(m => m.DisciplinaId == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // GET: Disciplina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplinaId,Naziv,Opis")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disciplina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplina
                                    .Include(d => d.Trener)
                                    .FirstOrDefaultAsync(d => d.DisciplinaId == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            var treneri = _context.Trener
                            .Select(t => new {
                                t.TrenerId,
                                ImePrezime = t.Ime + " " + t.Prezime
                            })
                            .ToList();

            ViewBag.SviTreneri = new SelectList(treneri, "TrenerId", "ImePrezime");

            return View(disciplina);
        }

        // POST: Disciplina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisciplinaId,Naziv,Opis")] Disciplina disciplina)
        {
            if (id != disciplina.DisciplinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinaExists(disciplina.DisciplinaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        [HttpPost]
        public async Task<IActionResult> EditMD(
    Disciplina disciplina,
    int? DodajTrenerId,
    int[]? TreneriZaBrisanje,
    List<int>? DodajTreneriId)
        {
            var disciplinaDb = await _context.Disciplina
                .Include(d => d.Trener)
                .FirstOrDefaultAsync(d => d.DisciplinaId == disciplina.DisciplinaId);

            if (disciplinaDb == null)
                return NotFound();

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

            return RedirectToAction(nameof(MasterDetails), new { id = disciplina.DisciplinaId });
        }


        // GET: Disciplina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplina
                .FirstOrDefaultAsync(m => m.DisciplinaId == id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disciplina = await _context.Disciplina.FindAsync(id);
            if (disciplina != null)
            {
                _context.Disciplina.Remove(disciplina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaExists(int id)
        {
            return _context.Disciplina.Any(e => e.DisciplinaId == id);
        }

        public async Task<IActionResult> MasterDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _context.Disciplina
                .Include(d => d.Trener)
                .Include(d => d.Clan)
                .Include(d => d.Trening)
                .FirstOrDefaultAsync(m => m.DisciplinaId == id);

            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var discipline = from d in _context.Disciplina
                             select d;

            if (!string.IsNullOrEmpty(searchString))
            {
                discipline = discipline.Where(d => d.Naziv.Contains(searchString));
            }

            return View(await discipline.ToListAsync());
        }

        [HttpPost]
        public IActionResult AddPostojeciClanToDisciplina(int DisciplinaId, int ClanId)
        {
            var disciplina = _context.Disciplina.Include(d => d.Clan)
                                .FirstOrDefault(d => d.DisciplinaId == DisciplinaId);
            var clan = _context.Clan.Find(ClanId);

            if (disciplina != null && clan != null && !disciplina.Clan.Contains(clan))
            {
                disciplina.Clan.Add(clan);
                _context.SaveChanges();
            }

            return RedirectToAction("MasterDetails", new { id = DisciplinaId });
        }

        [HttpPost]
        public IActionResult AddNoviClanToDisciplina(int DisciplinaId, string Ime, string Prezime, string Oib)
        {
            var clan = new Clan { Ime = Ime, Prezime = Prezime, Oib = Oib };
            _context.Clan.Add(clan);
            _context.SaveChanges();

            var disciplina = _context.Disciplina.Include(d => d.Clan)
                                .FirstOrDefault(d => d.DisciplinaId == DisciplinaId);

            if (disciplina != null)
            {
                disciplina.Clan.Add(clan);
                _context.SaveChanges();
            }

            return RedirectToAction("MasterDetails", new { id = DisciplinaId });
        }

        [HttpGet]
        public IActionResult AddNoviClanToDisciplina(int id) // id = DisciplinaId
        {
            ViewBag.DisciplinaId = id;
            return View();
        }

        [HttpGet]
        public IActionResult AddPostojeciClanToDisciplina(int id) // id = DisciplinaId
        {
            ViewBag.DisciplinaId = id;
            ViewBag.Clanovi = _context.Clan
                            .Select(c => new SelectListItem
                            {
                                Value = c.ClanId.ToString(),
                                Text = c.Ime + " " + c.Prezime
                            }).ToList();
            return View();
        }

        // DELETE: Disciplina/RemoveClan/5?clanId=10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UkloniClan(int disciplinaId, int clanId)
        {
            var disciplina = await _context.Disciplina
                .Include(d => d.Clan)
                .FirstOrDefaultAsync(d => d.DisciplinaId == disciplinaId);

            if (disciplina == null)
            {
                return NotFound("Disciplina nije pronađena.");
            }

            var clanToRemove = disciplina.Clan.FirstOrDefault(c => c.ClanId == clanId);
            if (clanToRemove == null)
            {
                return NotFound("Član nije pronađen u disciplini.");
            }

            disciplina.Clan.Remove(clanToRemove);

            await _context.SaveChangesAsync();

            return RedirectToAction("MasterDetails", new { id = disciplinaId });
        }

    }
}
