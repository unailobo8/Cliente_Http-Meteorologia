using Microsoft.EntityFrameworkCore;

namespace Baliza.Models
{
    public class BalizaContext : DbContext
    {
        public BalizaContext(DbContextOptions<BalizaContext> options)
            : base(options)
            {
            }
        
        public DbSet<Balizas2> Balizas2 {get; set;}
        public string connString {get; private set;}
        public BalizaContext()
        {
            connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database=DB12Unai;User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)=> options.UseSqlServer(connString);
    }
}