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
    }
}
