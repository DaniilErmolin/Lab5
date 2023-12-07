using System.ComponentModel.DataAnnotations;
using Domains.Models;
using Domains.ViewModels;

namespace Domains.ViewModel
{
    public class TypesOfRecreationViewModel
    {
        public SortViewModel SortViewModel { get; set; }
        public IEnumerable<TypesOfRecreation> TypesOfRecreations{ get; set;}

        public PageViewModel Page { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Ограничения")]
        public string Restrictions { get; set; }

    }
}
