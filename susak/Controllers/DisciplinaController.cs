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
        private readonly IDisciplinaRepository _disciplinaService;

        public DisciplinaController(IDisciplinaRepository disciplinaService)
        {
            _disciplinaService = disciplinaService;
        }

        // GET: Disciplina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var disciplina = await _disciplinaService.GetByIdAsync(id.Value);
            if (disciplina == null)
                return NotFound();

            return View(disciplina);
        }

        // GET: Disciplina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplinaId,Naziv,Opis")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                await _disciplinaService.AddAsync(disciplina);
                await _disciplinaService.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var disciplina = await _disciplinaService.GetByIdWithRelationsAsync(id.Value);
            if (disciplina == null)
                return NotFound();

            var treneriSelectList = _disciplinaService.GetTreneriSelectList();
            ViewBag.SviTreneri = new SelectList(treneriSelectList, "TrenerId", "ImePrezime");

            return View(disciplina);
        }

        // POST: Disciplina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisciplinaId,Naziv,Opis")] Disciplina disciplina)
        {
            if (id != disciplina.DisciplinaId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _disciplinaService.UpdateAsync(disciplina);
                    await _disciplinaService.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _disciplinaService.ExistsAsync(disciplina.DisciplinaId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // POST: Disciplina/EditMD
        [HttpPost]
        public async Task<IActionResult> EditMD(
            Disciplina disciplina,
            int? DodajTrenerId,
            int[]? TreneriZaBrisanje,
            List<int>? DodajTreneriId)
        {
            var disciplinaDb = await _disciplinaService.GetByIdWithRelationsAsync(disciplina.DisciplinaId);
            if (disciplinaDb == null)
                return NotFound();

            disciplinaDb.Naziv = disciplina.Naziv;
            disciplinaDb.Opis = disciplina.Opis;

            await _disciplinaService.EditMasterDetailAsync(disciplinaDb, DodajTrenerId, TreneriZaBrisanje, DodajTreneriId);

            await _disciplinaService.SaveAsync();

            return RedirectToAction(nameof(MasterDetails), new { id = disciplina.DisciplinaId });
        }

        // GET: Disciplina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var disciplina = await _disciplinaService.GetByIdAsync(id.Value);
            if (disciplina == null)
                return NotFound();

            return View(disciplina);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _disciplinaService.DeleteAsync(id);
            await _disciplinaService.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DisciplinaExists(int id)
        {
            return await _disciplinaService.ExistsAsync(id);
        }

        public async Task<IActionResult> MasterDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var disciplina = await _disciplinaService.GetByIdWithRelationsAsync(id.Value);
            if (disciplina == null)
                return NotFound();

            return View(disciplina);
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var discipline = await _disciplinaService.GetAllAsync(searchString);
            return View(discipline);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostojeciClanToDisciplina(int DisciplinaId, int ClanId)
        {
            var disciplina = await _disciplinaService.GetByIdWithRelationsAsync(DisciplinaId);
            if (disciplina == null)
                return NotFound();

            var clan = new Clan { ClanId = ClanId };

            _disciplinaService.AddPostojeciClan(DisciplinaId, ClanId);

            await _disciplinaService.SaveAsync();

            return RedirectToAction("MasterDetails", new { id = DisciplinaId });
        }

        [HttpPost]
        public async Task<IActionResult> AddNoviClanToDisciplina(int DisciplinaId, string Ime, string Prezime, string Oib)
        {
            var clan = new Clan { Ime = Ime, Prezime = Prezime, Oib = Oib };
            _disciplinaService.AddClanToDisciplina(DisciplinaId, clan);

            await _disciplinaService.SaveAsync();

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

            
            var clanovi = _disciplinaService.GetClanoviSelectList(); 
            ViewBag.Clanovi = clanovi.Select(c => new SelectListItem
            {
                Value = c.GetType().GetProperty("ClanId")?.GetValue(c)?.ToString(),
                Text = c.GetType().GetProperty("ImePrezime")?.GetValue(c)?.ToString()
            }).ToList();

            return View();
        }

        // POST: Disciplina/UkloniClan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UkloniClan(int disciplinaId, int clanId)
        {
            var disciplina = await _disciplinaService.GetByIdWithRelationsAsync(disciplinaId);
            if (disciplina == null)
                return NotFound("Disciplina nije pronađena.");

            var clanToRemove = disciplina.Clan.FirstOrDefault(c => c.ClanId == clanId);
            if (clanToRemove == null)
                return NotFound("Član nije pronađen u disciplini.");

            await _disciplinaService.RemoveClanAsync(disciplinaId, clanId);
            await _disciplinaService.SaveAsync();

            return RedirectToAction("MasterDetails", new { id = disciplinaId });
        }
    }
}
