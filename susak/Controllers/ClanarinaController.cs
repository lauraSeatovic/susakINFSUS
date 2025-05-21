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
    public class ClanarinaController : Controller
    {
        private readonly susakContext _context;

        public ClanarinaController(susakContext context)
        {
            _context = context;
        }

        // GET: Clanarina
        public async Task<IActionResult> Index()
        {
            var susakContext = _context.Clanarina.Include(c => c.Clan);
            return View(await susakContext.ToListAsync());
        }

        // GET: Clanarina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina
                .Include(c => c.Clan)
                .FirstOrDefaultAsync(m => m.ClanarinaId == id);
            if (clanarina == null)
            {
                return NotFound();
            }

            return View(clanarina);
        }

        // GET: Clanarina/Create
        public IActionResult Create()
        {
            ViewData["ClanId"] = new SelectList(_context.Clan, "ClanId", "ClanId");
            return View();
        }

        // POST: Clanarina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClanarinaId,ClanId,Iznos,DatumUplate,Status")] Clanarina clanarina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clanarina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClanId"] = new SelectList(_context.Clan, "ClanId", "ClanId", clanarina.ClanId);
            return View(clanarina);
        }

        // GET: Clanarina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina.FindAsync(id);
            if (clanarina == null)
            {
                return NotFound();
            }
            ViewData["ClanId"] = new SelectList(_context.Clan, "ClanId", "ClanId", clanarina.ClanId);
            return View(clanarina);
        }

        // POST: Clanarina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClanarinaId,ClanId,Iznos,DatumUplate,Status")] Clanarina clanarina)
        {
            if (id != clanarina.ClanarinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clanarina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanarinaExists(clanarina.ClanarinaId))
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
            ViewData["ClanId"] = new SelectList(_context.Clan, "ClanId", "ClanId", clanarina.ClanId);
            return View(clanarina);
        }

        // GET: Clanarina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina
                .Include(c => c.Clan)
                .FirstOrDefaultAsync(m => m.ClanarinaId == id);
            if (clanarina == null)
            {
                return NotFound();
            }

            return View(clanarina);
        }

        // POST: Clanarina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clanarina = await _context.Clanarina.FindAsync(id);
            if (clanarina != null)
            {
                _context.Clanarina.Remove(clanarina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanarinaExists(int id)
        {
            return _context.Clanarina.Any(e => e.ClanarinaId == id);
        }
    }
}
