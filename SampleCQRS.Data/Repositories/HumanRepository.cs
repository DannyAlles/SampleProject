using Microsoft.EntityFrameworkCore;
using SampleCQRS.Data.Interfaces;
using SampleCQRS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Data.Repositories
{
    public class HumanRepository : IHumanRepository
    {
        private readonly HumanDbContext _dbContext;

        public HumanRepository(HumanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateNewHumanAsync(Human newHuman)
        {
            await _dbContext.Humans.AddAsync(newHuman).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Human?> GetHumanByIdAsync(Guid id)
        {
            return await _dbContext.Humans.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Human>> GetHumansListAsync()
        {
            return await _dbContext.Humans.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateHumanAsync(Human updatedHuman)
        {
            _dbContext.Humans.Update(updatedHuman);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
