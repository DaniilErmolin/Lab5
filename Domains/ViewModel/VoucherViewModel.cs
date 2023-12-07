using System.ComponentModel.DataAnnotations;
using Domains.Models;

namespace Domains.ViewModel
{
    public class VoucherViewModel
    {
        public IEnumerable<Voucher> Vouchers { get; set;}

        public PageViewModel Page { get; set; }
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Отель")]
        public string HotelName { get; set; }
        [Display(Name = "Услуга")]
        public string TypeOfRecreationName { get; set; }
        [Display(Name = "Доп. услуга")]
        public string AdditionalServiceName { get; set; }
        [Display(Name = "Клиент")]
        public string ClientFio { get; set; }
        [Display(Name = "Работник")]
        public string EmployessFio { get; set; }
        [Display(Name = "Резервирование")]
        public bool Reservation { get; set; }
        [Display(Name = "Оплата")]
        public bool Payment { get; set; }

    }
}
