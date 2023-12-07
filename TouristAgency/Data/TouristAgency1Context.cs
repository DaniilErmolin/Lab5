using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domains.Models;

namespace TouristAgency.Data;

public partial class TouristAgency1Context : IdentityDbContext
{
    public TouristAgency1Context()
    {
    }

    public TouristAgency1Context(DbContextOptions<TouristAgency1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalService> AdditionalServices { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<TypesOfRecreation> TypesOfRecreations { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }


}
