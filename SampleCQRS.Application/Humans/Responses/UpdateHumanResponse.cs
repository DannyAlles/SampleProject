using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCQRS.Application.Humans.Responses
{
    public class UpdateHumanResponse
    {
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
