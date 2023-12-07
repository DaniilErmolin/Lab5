using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models;

public partial class Client
{
    public int Id { get; set; }
    [Display(Name = "ФИО")]
    public string Fio { get; set; } = null!;
    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Display(Name = "Пол")]
    public string Sex { get; set; } = null!;
    [Display(Name = "Адрес")]
    public string Address { get; set; } = null!;
    [Display(Name = "Серия")]
    public string Series { get; set; } = null!;
    [Display(Name = "Номер")]
    public long Number { get; set; }
    [Display(Name = "Скидка")]
    public long Discount { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
