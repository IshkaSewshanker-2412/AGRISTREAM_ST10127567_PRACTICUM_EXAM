using AgriStreamMVC.Models;
using Microsoft.EntityFrameworkCore;


namespace AgriStreamMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FarmSensor> FarmSensors { get; set; }
    }
}
