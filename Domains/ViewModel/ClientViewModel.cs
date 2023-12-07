using System.ComponentModel.DataAnnotations;
using Domains.Models;

namespace Domains.ViewModel
{
    public class ClientViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public PageViewModel Page { get; set; }
        [Display(Name = "ФИО")]
        public string Fio { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Пол")]
        public string Sex { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Серия")]
        public string Series { get; set; }
        [Display(Name = "Номер")]
        public long Number { get; set; }
        [Display(Name = "Скидка")]
        public long Discount { get; set; }


    }
}
