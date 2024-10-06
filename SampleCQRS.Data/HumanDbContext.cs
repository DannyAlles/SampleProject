using Microsoft.EntityFrameworkCore;
using SampleCQRS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Data
{
    public class HumanDbContext : DbContext
    {
        public HumanDbContext(DbContextOptions options) : base(options)
        {
        }

        protected HumanDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Human> Humans { get; set; }
    }
}
