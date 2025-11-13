using System.Reflection;
using Asp.Versioning;
using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using Catalog.Infrastracure.Data.Context;
using Catalog.Infrastracure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(CreateProductCommand))!));

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, CatalogRepository>();
builder.Services.AddScoped<IBrandRepository, CatalogRepository>();
builder.Services.AddScoped<ITypeRepository, CatalogRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "this api for Catalog microservice in an e-commerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Abanoub Saweris",
            Email = "abanoub.saweris02@gmail.com",
            Url = new Uri("https://github.com/AbanoubPhelopos/E-Commerce")
        }
    });
});
builder.Services.AddOpenApi();

var app = builder.Build();

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
