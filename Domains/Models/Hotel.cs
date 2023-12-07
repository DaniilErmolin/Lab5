using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models;

public partial class Hotel
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
    [Display(Name = "Страна")]
    public string Country { get; set; } = null!;
    [Display(Name = "Город")]
    public string City { get; set; } = null!;
    [Display(Name = "Адрес")]
    public string Address { get; set; } = null!;
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = null!;
    [Display(Name = "Звёзд")]
    public int Stars { get; set; }
    [Display(Name = "Контактное лицо")]
    public string TheContactPerson { get; set; } = null!;
    [Display(Name = "Фото")]
    public byte[]? Photo { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
