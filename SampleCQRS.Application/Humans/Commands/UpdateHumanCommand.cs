using MediatR;
using SampleCQRS.Application.Humans.Responses;

namespace SampleCQRS.Application.Humans.Commands
{
    public class UpdateHumanCommand : IRequest<UpdateHumanResponse>
    {
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
