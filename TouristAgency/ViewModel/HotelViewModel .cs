using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class HotelViewModel
    {
        public IEnumerable<Hotel> Hotels { get; set;}
        public PageViewModel Page { get; set;}

        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int Stars { get; set; }

        public string TheContactPerson { get; set; }
    }
}

