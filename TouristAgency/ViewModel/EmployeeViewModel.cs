using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class EmployeeViewModel
    {
        public IEnumerable<Employee> Employees { get; set;}

        public PageViewModel Page { get; set; }

        public string  Fio { get; set;}
        public string JobTitle { get; set; }

        public int Age { get; set; }

    }
}
