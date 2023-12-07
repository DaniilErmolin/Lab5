using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models;

public partial class Voucher
{
    public int Id { get; set; }
    [Display(Name = "Дата начала")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Display(Name = "Дата окончания")]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }
    [Display(Name = "Отель")]
    public int HotelId { get; set; }
    [Display(Name = "Услуга")]
    public int TypeOfRecreationId { get; set; }
    [Display(Name = "Доп. услуга")]
    public int AdditionalServiceId { get; set; }
    [Display(Name = "Клиент")]
    public int ClientId { get; set; }
    [Display(Name = "Сотрудник")]
    public int EmployessId { get; set; }
    [Display(Name = "Резервирование")]
    public bool Reservation { get; set; }
    [Display(Name = "Оплата")]
    public bool Payment { get; set; }

    public virtual AdditionalService? AdditionalService { get; set; } = null!;

    public virtual Client? Client { get; set; } = null!;

    public virtual Employee? Employess { get; set; } = null!;

    public virtual Hotel? Hotel { get; set; } = null!;

    public virtual TypesOfRecreation? TypeOfRecreation { get; set; } = null!;
}
