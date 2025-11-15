using ApiOne.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOne.Implementations
{
    public class ApiDbContext : DbContext 
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base (options)
        {
        }
        public DbSet<ClientInput> ClientInputs { get; set; }
    }
}
