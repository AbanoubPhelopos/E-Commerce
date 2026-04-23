using Asp.Versioning;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.OpenApi;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Logger.ConfigureLogger);


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "this api for Catalog microservice in an e-commerce application",
        Contact = new OpenApiContact
        {
            Name = "Abanoub Saweris",
            Email = "abanoub.saweris02@gmail.com",
            Url = new Uri("https://github.com/AbanoubPhelopos/E-Commerce")
        }
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


builder.Services.AddScoped<BasketOrderingConsumer>();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketOrderingConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();



builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

await app.MigrateDatabaseAsync<OrderContext>(async (context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
    await OrderContextSeed.SeedAsync(context, logger);
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
