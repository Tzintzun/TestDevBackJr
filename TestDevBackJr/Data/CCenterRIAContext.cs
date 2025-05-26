using Microsoft.EntityFrameworkCore;
using TestDevBackJr.Models;
using TestDevBackJr.Models.Dto;

namespace TestDevBackJr.Data
{
    public class CCenterRIAContext : DbContext
    {
        public CCenterRIAContext(DbContextOptions<CCenterRIAContext> options) : base(options) { }

        //Se agregan los modelos de Areas, User y Login, al DbContext
        public DbSet<Area> Areas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<UserHorasDto> UserHoras { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
