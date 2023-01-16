using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
    }
}