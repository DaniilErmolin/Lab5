using System.ComponentModel.DataAnnotations;
using Domains.Models;
using Domains.ViewModels;

namespace Domains.ViewModel
{
    public class AdditionalServiceViewModel
    {
        public SortViewModel SortViewModel { get; set; }
        public IEnumerable<AdditionalService> AdditionalServices { get; set;}
        public PageViewModel Page { get; set;}
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

    }
}
