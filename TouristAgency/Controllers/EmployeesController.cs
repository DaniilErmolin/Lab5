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
    public class EmployeesController : Controller
    {
        private readonly TouristAgency1Context _context;
        private readonly int pageSize = 10;

        public EmployeesController(TouristAgency1Context context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int page = 1)
        {
            EmployeeViewModel employeesModel;
            var employe = HttpContext.Session.Get<EmployeeViewModel>("Employee");
            if (employe == null)
            {
                employe = new EmployeeViewModel();
            }
            IQueryable<Employee> employeesDbContext = _context.Employees;
            employeesDbContext = SortSearch(employeesDbContext, employe.Fio, employe.JobTitle, employe.Age);
            var count = employeesDbContext.Count();
            employeesDbContext = employeesDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            employeesModel = new EmployeeViewModel
            {
                Page = new PageViewModel(count, page, pageSize),
                Employees = employeesDbContext,
                Fio = employe.Fio,
                JobTitle = employe.JobTitle,
                Age = employe.Age   
            };
            return View(employeesModel);
        }
        [HttpPost]
        public  IActionResult Index(EmployeeViewModel employee)
        {
            HttpContext.Session.Set("Employee", employee);

            return RedirectToAction("Index");

        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fio,JobTitle,Age")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fio,JobTitle,Age")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'TouristAgency1Context.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private IQueryable<Employee> SortSearch(IQueryable<Employee> employees, string searchFIO, string searchJobTitle, int age)
        {
            employees = employees.Where(e => e.Fio.Contains(searchFIO ?? "")
            && e.JobTitle.Contains(searchJobTitle ?? "")
            && (e.Age == age || age == 0));

            return employees;
        }
    }
}
