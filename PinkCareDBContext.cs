using Microsoft.EntityFrameworkCore;
using PinkCare.Models;

namespace PinkCare.Data
{
    public class PinkCareDBContext : DbContext
    {
        public PinkCareDBContext(DbContextOptions<PinkCareDBContext> options) 
            : base(options)
        {
        }
        public DbSet<UserInfo> UserInfos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration can go here
        }
    }
}
