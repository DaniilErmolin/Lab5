using System.ComponentModel.DataAnnotations;
using Domains.Models;

namespace Domains.ViewModel
{
    public class HotelViewModel
    {
        public IEnumerable<Hotel> Hotels { get; set;}
        public PageViewModel Page { get; set;}

        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Страна")]
        public string Country { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Звёзды")]
        public int Stars { get; set; }
        [Display(Name = "Контактное лицо")]
        public string TheContactPerson { get; set; }
    }
}

