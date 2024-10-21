using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Bus;
using SampleCQRS.Application.Humans.Commands;
using SampleCQRS.Application.Humans.Handlers;
using SampleCQRS.Application.Humans.Queries;
using SampleCQRS.Application.Humans.Responses;
using SampleCQRS.Data;
using SampleCQRS.Data.Interfaces;
using SampleCQRS.Data.Repositories;
using SampleCQRS.Infrastructure;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
const string _corsPolicy = "EnableAll";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sample"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _corsPolicy,
                        builder =>
                        {
                            builder
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .AllowAnyMethod();
                        });
});

builder.Services.AddDbContext<HumanDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru-RU");
    options.SupportedCultures = [new CultureInfo("ru-RU")];
    options.SupportedUICultures = [new CultureInfo("ru-RU")];
});

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);
ConfigureRepositories(builder.Services);
ConfigureHandlers(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

await ApplyMigrations(app).ConfigureAwait(false);

app.MapControllers();

app.Run();

static void ConfigureRepositories(IServiceCollection services)
{
    services.AddScoped<IHumanRepository, HumanRepository>();
}

static void ConfigureHandlers(IServiceCollection services)
{
    #region Commands

    services.AddScoped<IRequestHandler<CreateNewHumanCommand, CreateNewHumanResponse>, CreateNewHumanHandler>();
    services.AddScoped<IRequestHandler<UpdateHumanCommand, UpdateHumanResponse>, UpdateHumanHandler>();

    #endregion

    #region Queries

    services.AddScoped<IRequestHandler<GetHumansListQuery, IEnumerable<GetHumansListResponse>>, GetHumansListHandler>();
    services.AddScoped<IRequestHandler<GetHumanByIdQuery, GetHumanByIdResponse>, GetHumanByIdHandler>();

    #endregion
}

static async Task ApplyMigrations(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    using var db = scope.ServiceProvider.GetService<HumanDbContext>();
    await db.Database.MigrateAsync();
}