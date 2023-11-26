using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class ClientViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public PageViewModel Page { get; set; }

        public string Fio { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }

        public string Series { get; set; }

        public long Number { get; set; }

        public long Discount { get; set; }


    }
}
