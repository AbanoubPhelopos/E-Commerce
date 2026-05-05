using System.Reflection;
using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using MassTransit;
using Microsoft.OpenApi;
using static Discount.Grpc.Protos.DiscountProtoService;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Logger.ConfigureLogger);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(CreateShoppingCartCommand))!));

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});


builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "this api for Basket microservice in an e-commerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Abanoub Saweris",
            Email = "abanoub.saweris02@gmail.com",
            Url = new Uri("https://github.com/AbanoubPhelopos/E-Commerce")
        }
    });

    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v2",
        Description = "this api for Basket microservice in an e-commerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Abanoub Saweris",
            Email = "abanoub.saweris02@gmail.com",
            Url = new Uri("https://github.com/AbanoubPhelopos/E-Commerce")
        }
    });

    options.DocInclusionPredicate = (docName, description) =>
    {
        if (!description.TryGetMethodInfo(out var methodInfo))
            return false;
        var versions = methodInfo.DeclaringType?.GetCustomAttributes(true)?
                            .OfType<ApiVersionAttribute>().SelectMany(att => att.Versions);
        return versions.Any(v => $"v{v.ToString()}" == docName) ?? false;
    };
});

builder.Services.AddOpenApi();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket API V2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
