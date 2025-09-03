using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TGH.Helpers;
using TGH.Models;

namespace TGH.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationImage> DonationImages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            modelBuilder.Entity<ApplicationUser>().HasOne(o => o.City).WithMany(c => c.ApplicationUsers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Conversation>()
    .HasOne(x => x.Customer)
    .WithMany()
    .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<Conversation>()
                .HasOne(x => x.Donator)
                .WithMany()
                .HasForeignKey(p => p.DonatorId);
            modelBuilder.Entity<Conversation>().HasMany(n => n.Notifications).WithOne().HasForeignKey(x => x.RelatedItemID);

            

        }


    }

}
