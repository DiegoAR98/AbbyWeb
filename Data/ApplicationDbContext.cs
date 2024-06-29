using Abbyweb.Model;
using Microsoft.EntityFrameworkCore;

namespace Abbyweb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Category> Category { get; set; }
}