using HKDXX6_HFT_2023241.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace HKDXX6_HFT_2023241.Repository
{
    public class PoliceDbContext : DbContext
    {
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Precinct> Precincts { get; set; }
        public DbSet<Case> Cases { get; set; }

        public PoliceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseInMemoryDatabase("PoliceDB").UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Officer>(officer => officer
                .HasOne(officer => officer.Precinct) //Officer has a precinct
                .WithMany(precinct => precinct.Officers) //One precinct has many officers
                .HasForeignKey(officer => officer.PrecinctID) //FK assignment
                .OnDelete(DeleteBehavior.Restrict)); //If any officers are in precinct, it cannot be deleted

            modelBuilder.Entity<Officer>(officer => officer
                .HasOne(officer => officer.DirectCO) //Officer has a direct CO
                .WithMany(CO => CO.OfficersUnderCommand) //Who has multiple officers under command
                .HasForeignKey(officer => officer.DirectCO_BadgeNo) //FK assignment
                .OnDelete(DeleteBehavior.ClientSetNull)); //If CO is deleted, the officers CO is set to null

            modelBuilder.Entity<Case>()
                .HasOne(c => c.OfficerOnCase) //A case has an officer
                .WithMany(o => o.Cases) //An officer has many cases to which they are attached to
                .HasForeignKey(c => c.OfficerOnCaseID) //FK assignment
                .OnDelete(DeleteBehavior.Restrict) //Cannot delete officer if there are any cases to which they are attached to
                .IsRequired(false); // When a case is first added, it is not required to have a primary

            modelBuilder.Entity<Precinct>().HasData(new Precinct[]
            {
                new Precinct(93,"100 Meserole Avenue"),
                new Precinct(99,"211 Union Avenue")

            });

            modelBuilder.Entity<Officer>().HasData(new Officer[]
            {
                new Officer(1973,"Jack","Joel", Ranks.Captain,null,93,new DateTime(1980,01,01)),
                new Officer(3711,"David","Majors", Ranks.Detective,1973,93, new DateTime(2001,01,02)),
                new Officer(6382,"Raymond","Holt", Ranks.Captain,null,99, new DateTime(1980, 01, 02)),
                new Officer(378,"Terrence","Jeffords", Ranks.Sergeant,6382,99, new DateTime(1999, 03, 12)),
                new Officer(3263,"Amy","Santiago", Ranks.Sergeant,6382,99, new DateTime(2005, 02, 12)),
                new Officer(9544,"Jake","Peralta", Ranks.Detective,378,99, new DateTime(2004, 03, 10)),
                new Officer(426,"Charles","Boyle", Ranks.Detective,378,99, new DateTime(2000, 05, 16)),
                new Officer(3118,"Rosa","Diaz", Ranks.Detective,378,99, new DateTime(2004, 06, 18)),
                new Officer(18324,"Teri","Haver", Ranks.Officer,3263,99, new DateTime(2013, 08, 06)),
                new Officer(7529,"Lou","Vargas", Ranks.Officer,3263,99, new DateTime(2015, 03, 18)),
                new Officer(94499,"Gary","Jennings", Ranks.Officer,3263,99, new DateTime(2016, 11, 26))
            });

            modelBuilder.Entity<Case>().HasData(new Case[]
            {
                new Case(1,"Missing ham","A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544,new DateTime(2013,09,17,19,00,00)),
                new Case(2,"Blackmail of Parlov","Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript.",378, new DateTime(2013,09,17,19,00,00)),
                new Case(3,"Kidnapping of Cheddar the dog","Someone kidnapped the captain's dog, Cheddar (the fluffy boy), and demands ransom.",6382, new DateTime(2013, 09, 17, 19, 00, 00))
            });
        }

    }
}
