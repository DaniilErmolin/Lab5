using System.ComponentModel.DataAnnotations;
using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class VoucherViewModel
    {
        public IEnumerable<Voucher> Vouchers { get; set;}

        public PageViewModel Page { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public string HotelName { get; set; }

        public string TypeOfRecreationName { get; set; }

        public string AdditionalServiceName { get; set; }

        public string ClientFio { get; set; }

        public string EmployessFio { get; set; }

        public bool Reservation { get; set; }

        public bool Payment { get; set; }

    }
}
