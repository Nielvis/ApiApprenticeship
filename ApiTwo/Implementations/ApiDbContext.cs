using Microsoft.EntityFrameworkCore;
using ApiTwo.Models;

namespace ApiTwo.Implementations
{
    public class ApiDbContext : DbContext 
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base (options)
        {
        }
        public DbSet<TaskInput> TaskInputs { get; set; }
        public DbSet<TaskClient> TaskClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskClient>()
                .HasKey(tc => new { tc.ClientId, tc.TaskId });
            modelBuilder.Entity<TaskClient>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskClients)
                .HasForeignKey(tc => tc.TaskId);
        }
    }
}
