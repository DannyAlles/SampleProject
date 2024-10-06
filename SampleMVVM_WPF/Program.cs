using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Bus;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;
using SampleMVVM_WPF.EventBus.Events;
using SampleMVVM_WPF.Interfaces;
using SampleMVVM_WPF.Utilities;
using SampleMVVM_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM_WPF
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddHttpClient();

                    DependencyContainer.RegisterServices(services, configuration);

                    ConfigureUtilities(services);
                    ConfigureWindows(services);
                    ConfigureViewModels(services);
                })
                .Build();

            ConfigureEventBus(host);
            var app = host.Services.GetService<App>();

            app?.Run();
        }

        private static void ConfigureWindows(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
        }

        private static void ConfigureUtilities(IServiceCollection services)
        {
            services.AddScoped<IWebApi, WebApi>();
            services.AddScoped<IDialog, DefaultDialog>();
        }

        private static void ConfigureEventBus(IHost host)
        {
            var eventBus = host.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CreateNewHumanEvent, MainWindowViewModel>();
            eventBus.Subscribe<UpdateHumanByIdEvent, MainWindowViewModel>();
        }
    }
}
