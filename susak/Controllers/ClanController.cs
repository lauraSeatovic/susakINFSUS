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
    public class ClanController : Controller
    {
        private readonly susakContext _context;

        public ClanController(susakContext context)
        {
            _context = context;
        }

        // GET: Clan
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clan.ToListAsync());
        }

        // GET: Clan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.ClanId == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // GET: Clan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClanId,Ime,Prezime,Oib,DatumRodjenja,Adresa,Kontakt")] Clan clan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clan);
        }

        // GET: Clan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }
            return View(clan);
        }

        // POST: Clan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClanId,Ime,Prezime,Oib,DatumRodjenja,Adresa,Kontakt")] Clan clan)
        {
            if (id != clan.ClanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanExists(clan.ClanId))
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
            return View(clan);
        }

        // GET: Clan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.ClanId == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // POST: Clan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clan = await _context.Clan.FindAsync(id);
            if (clan != null)
            {
                _context.Clan.Remove(clan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanExists(int id)
        {
            return _context.Clan.Any(e => e.ClanId == id);
        }
    }
}
