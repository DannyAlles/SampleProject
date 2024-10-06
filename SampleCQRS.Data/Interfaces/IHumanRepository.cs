using SampleCQRS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Data.Interfaces
{
    public interface IHumanRepository
    {
        public Task CreateNewHumanAsync(Human newHuman);
        public Task UpdateHumanAsync(Human updatedHuman);
        public Task<Human?> GetHumanByIdAsync(Guid id);
        public Task<IEnumerable<Human>> GetHumansListAsync();
    }
}
