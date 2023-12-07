using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domains.ViewModel;
using TouristAgency.Infrastructure;
using Domains.Models;
using TouristAgency.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace TouristAgency.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;

        public ClientsController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(int page = 1)
        {
            ClientViewModel clientsModel;
            var client = HttpContext.Session.Get<ClientViewModel>("Client");
            if (client == null)
            {
                client = new ClientViewModel();
            }
            IQueryable<Client> clientsDbContext = _context.Clients;
            clientsDbContext = SortSearch(clientsDbContext, client.Fio, client.DateOfBirth, client.Sex, client.Address, client.Series,
                client.Number, client.Discount);
            var count = clientsDbContext.Count();
            clientsDbContext = clientsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            clientsModel = new ClientViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                Clients = clientsDbContext,
                Fio = client.Fio,
                DateOfBirth = client.DateOfBirth,
                Sex = client.Sex,
                Address = client.Address,
                Series = client.Series,
                Number = client.Number,
                Discount = client.Discount,
            };
            return View(clientsModel);
        }
        [HttpPost]
        public IActionResult Index(ClientViewModel client)
        {
            HttpContext.Session.Set("Client", client);

            return RedirectToAction("Index");

        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Fio,DateOfBirth,Sex,Address,Series,Number,Discount")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fio,DateOfBirth,Sex,Address,Series,Number,Discount")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'TouristAgency1Context.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Client> SortSearch(IQueryable<Client> clients, string Fio, DateTime DateOfBirth, string Sex, string Address,
            string Series, long Number, long Discount)
        {
            clients = clients.Where(e => e.Fio.Contains(Fio ?? "")
            && (e.DateOfBirth == DateOfBirth || DateOfBirth == new DateTime())
            && e.Sex.Contains(Sex ?? "")
            && e.Address.Contains(Address ?? "")
            && e.Series.Contains(Series ?? "")
            && (e.Number == Number || Number == 0)
            && (e.Discount == Discount || Discount == 0));

            return clients;
        }
    }
}
