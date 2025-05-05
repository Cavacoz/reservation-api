using Microsoft.EntityFrameworkCore;
using ReservationAPI.Models;

namespace ReservationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

    }

}