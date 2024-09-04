using EndoplasmCleanArchitecture.Domain.Common;
using EndoplasmCleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Presistence.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");

            //Seed some dummy data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "CfDJ8JvYfu6DtutMuOjp1Y0RCiPMgsJy9qEPnHwuQcrs49bpvitA_Xhq0-VU0v-aPmpkZmFl008B9ZxfIarGJ0YohG6HyDiRQKK5wUbKKWN1HADS4Pz3_eUI-fV2GPBEBa90Mw"
                });
        }

    }
}
