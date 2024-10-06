using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Bus
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<IEventBus, RabbitMQ.Bus.RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQ.Bus.RabbitMQBus(sp.GetService<IMediator>(),
                                                    configuration.GetSection("RabbitMQ:Host").Value,
                                                    Convert.ToInt32(configuration.GetSection("RabbitMQ:Port").Value),
                                                    configuration.GetSection("RabbitMQ:UserName").Value,
                                                    configuration.GetSection("RabbitMQ:Password").Value,
                                                    scopeFactory);
            });
        }
    }
}
