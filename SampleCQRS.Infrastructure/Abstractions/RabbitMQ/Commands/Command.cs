using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Events;
using System.ComponentModel.DataAnnotations;

namespace SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Commands
{
    public abstract class Command : Message
    {
        public DateTime TimeStamp { get; protected set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            TimeStamp = DateTime.UtcNow;
        }
    }
}
