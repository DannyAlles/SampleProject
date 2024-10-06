using System.ComponentModel.DataAnnotations;

namespace SampleCQRS.Api.ViewModels.Responses
{
    public class HumanViewModel
    {
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
