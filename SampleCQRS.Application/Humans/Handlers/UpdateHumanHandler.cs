using MediatR;
using SampleCQRS.Application.Exceptions;
using SampleCQRS.Application.Humans.Commands;
using SampleCQRS.Application.Humans.Events;
using SampleCQRS.Application.Humans.Responses;
using SampleCQRS.Data.Interfaces;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;

namespace SampleCQRS.Application.Humans.Handlers
{
    public class UpdateHumanHandler : IRequestHandler<UpdateHumanCommand, UpdateHumanResponse>
    {
        private readonly IEventBus _bus;
        private readonly IHumanRepository _humanRepository;

        public UpdateHumanHandler(IEventBus bus, IHumanRepository humanRepository)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _humanRepository = humanRepository ?? throw new ArgumentNullException(nameof(humanRepository));
        }
        public async Task<UpdateHumanResponse> Handle(UpdateHumanCommand request, CancellationToken cancellationToken)
        {
            var human = await _humanRepository.GetHumanByIdAsync(request.Id).ConfigureAwait(false) ?? throw new HumanNotFoundException();

            human.LastName = request.LastName;
            human.FirstName = request.FirstName;
            human.MiddleName = request.MiddleName;
            human.DateOfBirth = request.DateOfBirth;

            await _humanRepository.UpdateHumanAsync(human).ConfigureAwait(false);

            _bus.Publish(new UpdateHumanByIdEvent(human.Id, human.LastName, human.FirstName, human.MiddleName, human.DateOfBirth));

            return new UpdateHumanResponse()
            {
                Id = human.Id,
                LastName = human.LastName,
                FirstName = human.FirstName,
                MiddleName = human.MiddleName,
                DateOfBirth = human.DateOfBirth
            };
        }
    }
}
