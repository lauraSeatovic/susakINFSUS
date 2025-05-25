using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using susak.Models;

namespace susak.Controllers
{
    public class UlogaController : Controller
    {
        private readonly IUlogaService _ulogaService;

        public UlogaController(IUlogaService ulogaService)
        {
            _ulogaService = ulogaService;
        }

        public async Task<IActionResult> Index()
        {
            var uloge = await _ulogaService.GetAllAsync();
            return View(uloge);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var uloga = await _ulogaService.GetByIdAsync(id.Value);
            if (uloga == null) return NotFound();

            return View(uloga);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UlogaId,Naziv")] Uloga uloga)
        {
            if (!ModelState.IsValid)
                return View(uloga);

            await _ulogaService.AddAsync(uloga);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var uloga = await _ulogaService.GetByIdAsync(id.Value);
            if (uloga == null) return NotFound();

            return View(uloga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UlogaId,Naziv")] Uloga uloga)
        {
            if (id != uloga.UlogaId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(uloga);

            var exists = await _ulogaService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _ulogaService.UpdateAsync(uloga);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var uloga = await _ulogaService.GetByIdAsync(id.Value);
            if (uloga == null) return NotFound();

            return View(uloga);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exists = await _ulogaService.ExistsAsync(id);
            if (!exists)
                return NotFound();

            await _ulogaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
