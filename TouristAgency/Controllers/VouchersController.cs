using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domains.ViewModel;
using TouristAgency.Infrastructure;
using Domains.Models;
using TouristAgency.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TouristAgency.Controllers
{
    [Authorize]
    public class VouchersController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;

        public VouchersController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: Vouchers
        public async Task<IActionResult> Index(int page = 1)
        {
            VoucherViewModel vouchersModel;
            var voucher = HttpContext.Session.Get<VoucherViewModel>("Voucher");
            if (voucher == null)
            {
                voucher = new VoucherViewModel();
            }
            IQueryable<Voucher> vouchersDbContext = _context.Vouchers;
            vouchersDbContext = SortSearch(vouchersDbContext, voucher.StartDate, voucher.ExpirationDate, voucher.HotelName, voucher.TypeOfRecreationName,
                voucher.AdditionalServiceName, voucher.ClientFio, voucher.EmployessFio,  voucher.Payment, voucher.Reservation);
            var count = vouchersDbContext.Count();
            vouchersDbContext = vouchersDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            vouchersModel = new VoucherViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                Vouchers = vouchersDbContext,
                StartDate = voucher.StartDate,
                ExpirationDate = voucher.ExpirationDate,
                HotelName = voucher.HotelName,
                TypeOfRecreationName = voucher.TypeOfRecreationName,
                AdditionalServiceName = voucher.AdditionalServiceName,
                ClientFio = voucher.ClientFio,
                EmployessFio = voucher.EmployessFio,
                Reservation = voucher.Reservation,
                Payment = voucher.Payment,
            };
            return View(vouchersModel);
        }
        [HttpPost]
        public IActionResult Index(VoucherViewModel voucher)
        {
            HttpContext.Session.Set("Voucher", voucher);

            return RedirectToAction("Index");

        }

        // GET: Vouchers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .Include(v => v.AdditionalService)
                .Include(v => v.Client)
                .Include(v => v.Employess)
                .Include(v => v.Hotel)
                .Include(v => v.TypeOfRecreation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Vouchers/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "Id", "Name");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Fio");
            ViewData["EmployessId"] = new SelectList(_context.Employees, "Id", "Fio");
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            ViewData["TypeOfRecreationId"] = new SelectList(_context.TypesOfRecreations, "Id", "Name");
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,StartDate,ExpirationDate,HotelId,TypeOfRecreationId,AdditionalServiceId,ClientId,EmployessId,Reservation,Payment")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "Id", "Name", voucher.AdditionalServiceId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Fio", voucher.ClientId);
            ViewData["EmployessId"] = new SelectList(_context.Employees, "Id", "Fio", voucher.EmployessId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", voucher.HotelId);
            ViewData["TypeOfRecreationId"] = new SelectList(_context.TypesOfRecreations, "Id", "Name", voucher.TypeOfRecreationId);
            return View(voucher);
        }

        // GET: Vouchers/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "Id", "Name", voucher.AdditionalServiceId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Fio", voucher.ClientId);
            ViewData["EmployessId"] = new SelectList(_context.Employees, "Id", "Fio", voucher.EmployessId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", voucher.HotelId);
            ViewData["TypeOfRecreationId"] = new SelectList(_context.TypesOfRecreations, "Id", "Name", voucher.TypeOfRecreationId);
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,ExpirationDate,HotelId,TypeOfRecreationId,AdditionalServiceId,ClientId,EmployessId,Reservation,Payment")] Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Id))
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
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "Id", "Name", voucher.AdditionalServiceId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Fio", voucher.ClientId);
            ViewData["EmployessId"] = new SelectList(_context.Employees, "Id", "Fio", voucher.EmployessId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", voucher.HotelId);
            ViewData["TypeOfRecreationId"] = new SelectList(_context.TypesOfRecreations, "Id", "Name", voucher.TypeOfRecreationId);
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .Include(v => v.AdditionalService)
                .Include(v => v.Client)
                .Include(v => v.Employess)
                .Include(v => v.Hotel)
                .Include(v => v.TypeOfRecreation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'TouristAgency1Context.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(int id)
        {
          return (_context.Vouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Voucher> SortSearch(IQueryable<Voucher> employees, DateTime searchStartDate, DateTime searchExpirationDate, string HotelName, string TypeOfrecrationName, string AdditionalServiceName,
            string ClientFio, string EmployessFio, bool Payment, bool Reservation)
        {
            employees = employees.Include(h => h.Hotel).Include(h => h.TypeOfRecreation).Include(h => h.AdditionalService).Include(h => h.Client).Include(h => h.Employess)
                .Where(e => (e.StartDate == searchStartDate || searchStartDate == new DateTime())
            && (e.ExpirationDate == searchExpirationDate || searchExpirationDate == new DateTime())
            && e.Hotel.Name.Contains(HotelName ?? "")
            && e.TypeOfRecreation.Name.Contains(TypeOfrecrationName ?? "")
            && e.AdditionalService.Name.Contains(AdditionalServiceName ?? "")
            && e.Client.Fio.Contains(ClientFio ?? "")
            && e.Employess.Fio.Contains(EmployessFio ?? "")
            && (e.Payment == Payment || Payment ==false)
            && (e.Reservation == Reservation || Reservation ==false) );

            return employees;
        }
    }
}
