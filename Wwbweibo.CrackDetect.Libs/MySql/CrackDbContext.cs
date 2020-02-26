using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;
using Task = System.Threading.Tasks.Task;

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
