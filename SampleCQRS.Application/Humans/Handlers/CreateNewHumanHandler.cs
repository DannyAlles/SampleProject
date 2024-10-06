using MediatR;
using SampleCQRS.Application.Humans.Commands;
using SampleCQRS.Application.Humans.Events;
using SampleCQRS.Application.Humans.Responses;
using SampleCQRS.Data.Interfaces;
using SampleCQRS.Data.Models;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;

namespace SampleCQRS.Application.Humans.Handlers
{
    public class CreateNewHumanHandler : IRequestHandler<CreateNewHumanCommand, CreateNewHumanResponse>
    {
        private readonly IEventBus _bus;
        private readonly IHumanRepository _humanRepository;

        public CreateNewHumanHandler(IEventBus bus, IHumanRepository humanRepository)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _humanRepository = humanRepository ?? throw new ArgumentNullException(nameof(humanRepository));
        }

        public async Task<CreateNewHumanResponse> Handle(CreateNewHumanCommand request, CancellationToken cancellationToken)
        {
            var newHuman = new Human()
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                DateOfBirth = request.DateOfBirth
            };

            await _humanRepository.CreateNewHumanAsync(newHuman).ConfigureAwait(false);

            _bus.Publish(new CreateNewHumanEvent(newHuman.Id, newHuman.LastName, newHuman.FirstName, newHuman.MiddleName, newHuman.DateOfBirth));

            return new CreateNewHumanResponse()
            {
                Id = newHuman.Id,
                LastName = newHuman.LastName,
                FirstName = newHuman.FirstName,
                MiddleName = newHuman.MiddleName,
                DateOfBirth = newHuman.DateOfBirth
            };
        }
    }
}
