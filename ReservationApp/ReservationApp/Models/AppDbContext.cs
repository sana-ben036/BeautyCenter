using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {


        }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // si on veut initialiser la base de donnee et inserer des enregistrement 
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.Seed();


            // this code pour eviter la suppression d'un role qui est deja affecté to users , il faut pas permetter la suppression si le role est deja utiliser comme FK dans la table users

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }




        }
    }
}
