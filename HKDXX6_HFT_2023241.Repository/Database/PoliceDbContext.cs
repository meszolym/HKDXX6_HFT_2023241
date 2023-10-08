using HKDXX6_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HKDXX6_HFT_2023241.Repository
{
    public class PoliceDbContext : DbContext
    {
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Precinct> Precincts { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<OfficerOnCase> Officer_x_Case { get; set; }

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
                .HasMany(x => x.Officers) //A case has multiple officers
                .WithMany(x => x.Cases) //An officer has multiple cases
                .UsingEntity<OfficerOnCase>( //Using this connector table
                    x => x.HasOne(x => x.Officer) //OfficerOnCase has an officer
                        .WithMany() //The officer has many OfficerOnCase entries => Many cases
                        .HasForeignKey(x => x.OfficerBadgeNo) //FK assignment
                        .OnDelete(DeleteBehavior.Cascade), //If officer is deleted, delete the OfficerOnCase entries
                    x => x.HasOne(x => x.Case) //OfficerOnCase has a case
                        .WithMany() //The case has many OfficerOnCase entries => Many officers
                        .HasForeignKey(x => x.CaseID) //FK assignment
                        .OnDelete(DeleteBehavior.Cascade)); //If case is deleted, delete the OfficerOnCase entries



            modelBuilder.Entity<Precinct>().HasData(new Precinct[]
            {
                new Precinct(93,"100 Meserole Avenue"),
                new Precinct(99,"211 Union Avenue")

            });

            modelBuilder.Entity<Officer>().HasData(new Officer[]
            {
                new Officer(1973,"Jack","Joel", Ranks.Captain,null,93),
                new Officer(3711,"David","Majors", Ranks.Detective,1973,93),
                new Officer(6382,"Raymond","Holt", Ranks.Captain,null,99),
                new Officer(378,"Terrence","Jeffords", Ranks.Sergeant,6382,99),
                new Officer(3263,"Amy","Santiago", Ranks.Sergeant,6382,99),
                new Officer(9544,"Jake","Peralta", Ranks.Detective,378,99),
                new Officer(426,"Charles","Boyle", Ranks.Detective,378,99),
                new Officer(3118,"Rosa","Diaz", Ranks.Detective,378,99),
                new Officer(18324,"Teri","Haver", Ranks.PatrolOfficer,3263,99),
                new Officer(7529,"Lou","Vargas", Ranks.PatrolOfficer,3263,99),
                new Officer(94499,"Gary","Jennings", Ranks.PatrolOfficer,3263,99)
            });

            modelBuilder.Entity<Case>().HasData(new Case[]
            {
                new Case(1,"Missing ham","A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain."),
                new Case(2,"Blackmail of Parlov","Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript."),
                new Case(3,"Kidnapping of Cheddar the dog","Someone kidnapped the captain's dog, Cheddar (the fluffy boy), and demands ransom.")
            });

            modelBuilder.Entity<OfficerOnCase>().HasData(new OfficerOnCase[]
            {
                new OfficerOnCase(1,1,9544,true),
                new OfficerOnCase(2,1,426,false),
                new OfficerOnCase(3,2,378,true),
                new OfficerOnCase(4,2,9544,false),
                new OfficerOnCase(5,2,3118,false),
                new OfficerOnCase(6,3,6382,true),
                new OfficerOnCase(7,3,9544,false),
                new OfficerOnCase(8,3,426,false)

            });

        }

    }
}
