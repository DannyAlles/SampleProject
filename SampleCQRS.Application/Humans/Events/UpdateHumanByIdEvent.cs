using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Events;

namespace SampleCQRS.Application.Humans.Events
{
    public class UpdateHumanByIdEvent(Guid id, string lastName, string firstName, string middleName, DateTime dateOfbirth) : Event
    {
        public Guid Id { get; private set; } = id;

        public string LastName { get; private set; } = lastName;

        public string FirstName { get; private set; } = firstName;

        public string MiddleName { get; private set; } = middleName;

        public DateTime DateOfBirth { get; private set; } = dateOfbirth;
    }
}
