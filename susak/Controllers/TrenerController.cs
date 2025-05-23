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
    public class TrenerController : Controller
    {
        private readonly susakContext _context;

        public TrenerController(susakContext context)
        {
            _context = context;
        }

        // GET: Trener
        public async Task<IActionResult> Index()
        {
            var susakContext = _context.Trener.Include(t => t.Korisnik);
            return View(await susakContext.ToListAsync());
        }

        // GET: Trener/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener
                .Include(t => t.Korisnik)
                .FirstOrDefaultAsync(m => m.TrenerId == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // GET: Trener/Create
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme");
            return View();
        }

        // POST: Trener/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrenerId,Ime,Prezime,StrucnaSprema,KorisnikId")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trener);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
            return View(trener);
        }

        // GET: Trener/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
            return View(trener);
        }

        // POST: Trener/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrenerId,Ime,Prezime,StrucnaSprema,KorisnikId")] Trener trener)
        {
            if (id != trener.TrenerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trener);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrenerExists(trener.TrenerId))
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "KorisnikId", "KorisnickoIme", trener.KorisnikId);
            return View(trener);
        }

        // GET: Trener/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener
                .Include(t => t.Korisnik)
                .FirstOrDefaultAsync(m => m.TrenerId == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // POST: Trener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trener = await _context.Trener.FindAsync(id);
            if (trener != null)
            {
                _context.Trener.Remove(trener);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrenerExists(int id)
        {
            return _context.Trener.Any(e => e.TrenerId == id);
        }
    }
}
