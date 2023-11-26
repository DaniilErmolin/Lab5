﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TouristAgency.Models;

public partial class Voucher
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    public int HotelId { get; set; }

    public int TypeOfRecreationId { get; set; }

    public int AdditionalServiceId { get; set; }

    public int ClientId { get; set; }

    public int EmployessId { get; set; }

    public bool Reservation { get; set; }

    public bool Payment { get; set; }

    public virtual AdditionalService? AdditionalService { get; set; } = null!;

    public virtual Client? Client { get; set; } = null!;

    public virtual Employee? Employess { get; set; } = null!;

    public virtual Hotel? Hotel { get; set; } = null!;

    public virtual TypesOfRecreation? TypeOfRecreation { get; set; } = null!;
}