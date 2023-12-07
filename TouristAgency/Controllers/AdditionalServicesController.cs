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
    public class AdditionalServicesController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public AdditionalServicesController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: AdditionalServices
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            AdditionalServiceViewModel additionalServicesModel;
            var additionalService = HttpContext.Session.Get<AdditionalServiceViewModel>("AdditionalService");
            if (additionalService == null)
            {
                additionalService = new AdditionalServiceViewModel();
            }
            IQueryable<AdditionalService> additionalServicesDbContext = _context.AdditionalServices;
            additionalServicesDbContext = SortSearch(sortOrder, additionalServicesDbContext, additionalService.Name, additionalService.Description, additionalService.Price);
            // Разбиение на страницы
            var count = additionalServicesDbContext.Count();
            additionalServicesDbContext = additionalServicesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            additionalServicesModel = new AdditionalServiceViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                AdditionalServices = additionalServicesDbContext,
                Name = additionalService.Name,
                Description = additionalService.Description,
                Price = additionalService.Price,
                SortViewModel = new SortViewModel(sortOrder)
            };
            return View(additionalServicesModel);
        }
        [HttpPost]
        public IActionResult Index(AdditionalServiceViewModel additional)
        {
            HttpContext.Session.Set("AdditionalService", additional);

            return RedirectToAction("Index");

        }

        // GET: AdditionalServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdditionalServices == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (additionalService == null)
            {
                return NotFound();
            }

            return View(additionalService);
        }

        // GET: AdditionalServices/Create
        [Authorize(Roles ="admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdditionalServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] AdditionalService additionalService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(additionalService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(additionalService);
        }

        // GET: AdditionalServices/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdditionalServices == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService == null)
            {
                return NotFound();
            }
            return View(additionalService);
        }

        // POST: AdditionalServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] AdditionalService additionalService)
        {
            if (id != additionalService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(additionalService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdditionalServiceExists(additionalService.Id))
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
            return View(additionalService);
        }

        // GET: AdditionalServices/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdditionalServices == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (additionalService == null)
            {
                return NotFound();
            }

            return View(additionalService);
        }

        // POST: AdditionalServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdditionalServices == null)
            {
                return Problem("Entity set 'TouristAgency1Context.AdditionalServices'  is null.");
            }
            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService != null)
            {
                _context.AdditionalServices.Remove(additionalService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdditionalServiceExists(int id)
        {
          return (_context.AdditionalServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<AdditionalService> SortSearch(SortState sortOrder, IQueryable<AdditionalService> additionalServices, string searchName, string searchDescription, decimal price)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    additionalServices = additionalServices.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    additionalServices = additionalServices.OrderByDescending(s => s.Name);
                    break;
                case SortState.DescriptionAsc:
                    additionalServices = additionalServices.OrderBy(s => s.Description);
                    break;
                case SortState.DescriptionDesc:
                    additionalServices = additionalServices.OrderByDescending(s => s.Description);
                    break;

            }

            additionalServices = additionalServices.Where(e => e.Name.Contains(searchName ?? "")
            && e.Description.Contains(searchDescription ?? "")
            && (e.Price == price|| price == 0));

            return additionalServices;
        }
    }
}
