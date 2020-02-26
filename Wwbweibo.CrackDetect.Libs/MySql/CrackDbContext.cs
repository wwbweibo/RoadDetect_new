using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Crmf;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;
using Task = System.Threading.Tasks.Task;

namespace Wwbweibo.CrackDetect.Libs.MySql
{
    public class CrackDbContext : DbContext
    {
        public DbSet<Entity.Task> Tasks { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        public CrackDbContext(DbContextOptions<CrackDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public async Task<bool> CreateTask(Entity.Task task)
        {
            Tasks.Add(task);
            return await SaveChangesAsync() > 1;
        }

        public Entity.Task GetTask(string id)
        {
            return Tasks.FirstOrDefault(p => p.Id == id);
        }
    }
}
