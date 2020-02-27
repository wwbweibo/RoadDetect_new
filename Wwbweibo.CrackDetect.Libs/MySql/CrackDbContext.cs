using Microsoft.EntityFrameworkCore;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;

namespace Wwbweibo.CrackDetect.Libs.MySql
{
    public class CrackDbContext : DbContext
    {
        public static object obj = new object();
        public DbSet<Entity.Task> Tasks { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        public CrackDbContext(DbContextOptions<CrackDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
