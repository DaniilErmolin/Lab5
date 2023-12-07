using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models;

public partial class Employee
{
    public int Id { get; set; }
    [Display(Name = "ФИО")]
    public string Fio { get; set; } = null!;
    [Display(Name = "Должность")]
    public string JobTitle { get; set; } = null!;
    [Display(Name = "Возраст")]
    public int Age { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
