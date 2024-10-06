using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Data
{
    public class HumanDbContextFactory : IDesignTimeDbContextFactory<HumanDbContext>
    {
        public HumanDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HumanDbContext>();
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5433;User ID=postgres;Password=postgres;database=sample;");

            return new HumanDbContext(optionsBuilder.Options);
        }
    }
}
