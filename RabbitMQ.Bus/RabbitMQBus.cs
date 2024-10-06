using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Commands;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Events;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;
using System.Text;

namespace RabbitMQ.Bus
{
    public class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly List<Type> _evenTypes;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, string hostName, int port, string userName, string password, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _evenTypes = [];
            _handlers = [];
            _hostName = hostName;
            _port = port;
            _userName = userName;
            _password = password;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var eventName = @event.GetType().Name;
            
            channel.ExchangeDeclare(exchange: eventName, type: ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(eventName, string.Empty, null, body);
        }

        public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_evenTypes.Contains(typeof(T)))
                _evenTypes.Add(typeof(T));

            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, []);

            if (_handlers[eventName].Any(s => s == handlerType))
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));

            _handlers[eventName].Add(handlerType);

            StartBasicConsumer<T>();

        }

        public Task SendCommand<T>(T command) where T : Command => _mediator.Send(command);

        private void StartBasicConsumer<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.ExchangeDeclare(exchange: eventName, type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare(durable: true, autoDelete: false, exclusive: false, arguments: new Dictionary<string, object> { { "x-queue-type", "classic" } }).QueueName;

            channel.QueueBind(queue: queueName,
                              exchange: eventName,
                              routingKey: string.Empty);

            //channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.Exchange;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
                ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception exception)
            {
                throw new Exception($"Consumer exception: {exception.Message}");
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_handlers.TryGetValue(eventName, out List<Type>? value)) return;

            using var scope = _serviceScopeFactory.CreateScope();
            var subscriptions = value;

            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription);

                if (handler == null) continue;

                var eventType = _evenTypes.SingleOrDefault(t => t.Name == eventName);
                var @event = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle").Invoke(handler, [@event]);
            }
        }
    }
}
