using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class AdditionalServiceViewModel
    {
        public IEnumerable<AdditionalService> AdditionalServices { get; set;}
        public PageViewModel Page { get; set;}

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

    }
}
