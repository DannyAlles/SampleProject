using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Commands;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Events;

namespace SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
