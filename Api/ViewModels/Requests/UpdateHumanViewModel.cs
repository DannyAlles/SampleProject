using System.ComponentModel.DataAnnotations;

namespace SampleCQRS_MediatR.ViewModels.Requests
{
    public class UpdateHumanViewModel
    {
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string MiddleName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
