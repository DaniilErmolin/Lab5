using TouristAgency.Models;

namespace TouristAgency.ViewModel
{
    public class TypesOfRecreationViewModel
    {
        public IEnumerable<TypesOfRecreation> TypesOfRecreations{ get; set;}

        public PageViewModel Page { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Restrictions { get; set; }

    }
}
