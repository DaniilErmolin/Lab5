using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristAgency.ViewModel;
using TouristAgency.Infrastructure;
using TouristAgency.Models;
using TouristAgency.Data;
using Microsoft.AspNetCore.Authorization;

namespace TouristAgency.Controllers
{
    [Authorize]
    public class HotelsController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;

        public HotelsController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: Hotels
        public async Task<IActionResult> Index(int page = 1)
        {
            HotelViewModel hotelModel;
            var hotel = HttpContext.Session.Get<HotelViewModel>("Hotel");
            if (hotel == null)
            {
                hotel = new HotelViewModel();
            }
            IQueryable<Hotel> hotelsDbContext = _context.Hotels;
            hotelsDbContext = SortSearch(hotelsDbContext, hotel.Name, hotel.Country, hotel.City, hotel.Address, hotel.Phone, hotel.Stars, hotel.TheContactPerson);
            // Разбиение на страницы
            var count = hotelsDbContext.Count();
            hotelsDbContext = hotelsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            hotelModel = new HotelViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                Hotels = hotelsDbContext,
                Name = hotel.Name,
                Country = hotel.Country,
                City = hotel.City,
                Address = hotel.Address,
                Phone = hotel.Phone,
                Stars = hotel.Stars,
                TheContactPerson = hotel.TheContactPerson
            };
            return View(hotelModel);
        }
        [HttpPost]
        public IActionResult Index(HotelViewModel hotels)
        {
            HttpContext.Session.Set("Hotel", hotels);

            return RedirectToAction("Index");

        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,City,Address,Phone,Stars,TheContactPerson,Photo")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,City,Address,Phone,Stars,TheContactPerson,Photo")] Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Id))
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
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'TouristAgency1Context.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
          return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Hotel> SortSearch(IQueryable<Hotel> hotels, string searchName, string Country, string City, string Address, string Phone,
           int Stars, string TheContactPerson)
        {
            hotels = hotels.Where(e => e.Name.Contains(searchName ?? "")
            && e.Country.Contains(Country ?? "")
            && e.City.Contains(City ?? "")
            && e.Address.Contains(Address ?? "")
            && e.Phone.Contains(Phone ?? "")
            && (e.Stars == Stars || Stars == 0)
            && e.TheContactPerson.Contains(TheContactPerson ?? ""));

            return hotels;
        }
    }
}
