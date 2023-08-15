using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class XtraWorkContext : IdentityDbContext
    {
        public XtraWorkContext(DbContextOptions<XtraWorkContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}