using Azure.Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        
            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
            public DbSet<Appointment> Appointments { get; set; }
            public DbSet<Specialization> Specializations { get; set; }
            public DbSet<Booking> Bookings { get; set; }
            public DbSet<Discount> Discounts { get; set; }
            public DbSet<Coupon> Coupons { get; set; }
            public DbSet<Day> DoctorDays { get; set; }
            public DbSet<Time> DoctorTime { get; set; }




            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // Seed default roles
                SeedRoles(builder);
            }

            private void SeedRoles(ModelBuilder builder)
            {
                // Define default roles
                string[] roleNames = { "Admin", "Doctor", "Patient" };

                foreach (var roleName in roleNames)
                {
                    builder.Entity<IdentityRole>().HasData(new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });
                }
            }

        

    }
}
