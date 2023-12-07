using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models;

public partial class TypesOfRecreation
{
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;
    [Display(Name = "Ограничения")]
    public string Restrictions { get; set; } = null!;

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
