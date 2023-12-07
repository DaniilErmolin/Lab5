using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domains.ViewModel;
using TouristAgency.Infrastructure;
using Domains.Models;
using TouristAgency.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Domains.ViewModels;

namespace TouristAgency.Controllers
{
    [Authorize]
    public class TypesOfRecreationsController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;

        public TypesOfRecreationsController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: TypesOfRecreations
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            TypesOfRecreationViewModel typesOfRecreationModel;
            var typesOfRecreationService = HttpContext.Session.Get<TypesOfRecreationViewModel>("TypesOfRecreation");
            if (typesOfRecreationService == null)
            {
                typesOfRecreationService = new TypesOfRecreationViewModel();
            }
            IQueryable<TypesOfRecreation> typesDbContext = _context.TypesOfRecreations;
            typesDbContext = SortSearch(sortOrder, typesDbContext, typesOfRecreationService.Name, typesOfRecreationService.Description, typesOfRecreationService.Restrictions);
            var count = typesDbContext.Count();
            typesDbContext = typesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            typesOfRecreationModel = new TypesOfRecreationViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                TypesOfRecreations = typesDbContext,
                Name = typesOfRecreationService.Name,
                Description = typesOfRecreationService.Description,
                Restrictions = typesOfRecreationService.Restrictions,
                SortViewModel = new SortViewModel(sortOrder)
            };
            return View(typesOfRecreationModel);
        }
        [HttpPost]
        public IActionResult Index(TypesOfRecreationViewModel types)
        {
            HttpContext.Session.Set("TypesOfRecreation", types);

            return RedirectToAction("Index");

        }

        // GET: TypesOfRecreations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypesOfRecreations == null)
            {
                return NotFound();
            }

            var typesOfRecreation = await _context.TypesOfRecreations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesOfRecreation == null)
            {
                return NotFound();
            }

            return View(typesOfRecreation);
        }

        // GET: TypesOfRecreations/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypesOfRecreations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Restrictions")] TypesOfRecreation typesOfRecreation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typesOfRecreation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typesOfRecreation);
        }

        // GET: TypesOfRecreations/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypesOfRecreations == null)
            {
                return NotFound();
            }

            var typesOfRecreation = await _context.TypesOfRecreations.FindAsync(id);
            if (typesOfRecreation == null)
            {
                return NotFound();
            }
            return View(typesOfRecreation);
        }

        // POST: TypesOfRecreations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Restrictions")] TypesOfRecreation typesOfRecreation)
        {
            if (id != typesOfRecreation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typesOfRecreation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesOfRecreationExists(typesOfRecreation.Id))
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
            return View(typesOfRecreation);
        }

        // GET: TypesOfRecreations/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypesOfRecreations == null)
            {
                return NotFound();
            }

            var typesOfRecreation = await _context.TypesOfRecreations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesOfRecreation == null)
            {
                return NotFound();
            }

            return View(typesOfRecreation);
        }

        // POST: TypesOfRecreations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypesOfRecreations == null)
            {
                return Problem("Entity set 'TouristAgency1Context.TypesOfRecreations'  is null.");
            }
            var typesOfRecreation = await _context.TypesOfRecreations.FindAsync(id);
            if (typesOfRecreation != null)
            {
                _context.TypesOfRecreations.Remove(typesOfRecreation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesOfRecreationExists(int id)
        {
          return (_context.TypesOfRecreations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<TypesOfRecreation> SortSearch(SortState sortOrder, IQueryable<TypesOfRecreation> typesOfRecreations, string searchName, string searchDescription, string searchRecreation)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    typesOfRecreations = typesOfRecreations.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    typesOfRecreations = typesOfRecreations.OrderByDescending(s => s.Name);
                    break;
                case SortState.DescriptionAsc:
                    typesOfRecreations = typesOfRecreations.OrderBy(s => s.Description);
                    break;
                case SortState.DescriptionDesc:
                    typesOfRecreations = typesOfRecreations.OrderByDescending(s => s.Description);
                    break;

            }
            typesOfRecreations = typesOfRecreations.Where(e => e.Name.Contains(searchName ?? "")
            && e.Description.Contains(searchDescription ?? "")
            && e.Restrictions.Contains(searchRecreation ?? ""));

            return typesOfRecreations;
        }
    }
}
