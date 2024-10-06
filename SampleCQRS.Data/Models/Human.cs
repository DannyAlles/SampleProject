using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Data.Models
{
    public class Human
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
