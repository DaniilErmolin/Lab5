using System.ComponentModel.DataAnnotations;
using Domains.Models;

namespace Domains.ViewModel
{
    public class EmployeeViewModel
    {
        public IEnumerable<Employee> Employees { get; set;}

        public PageViewModel Page { get; set; }
        [Display(Name = "ФИО")]
        public string  Fio { get; set;}
        [Display(Name = "Должность")]
        public string JobTitle { get; set; }
        [Display(Name = "Возраст")]
        public int Age { get; set; }

    }
}
