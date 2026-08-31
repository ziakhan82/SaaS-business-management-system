using FluentValidation;
using ServiceFlow.Api.ExceptionHandling;
using ServiceFlow.Application.DTOs.Customers;
using ServiceFlow.Infrastructure;
using ServiceFlow.Application.Interfaces.Services;
using ServiceFlow.Application.Services;
using ServiceFlow.Application.Validators.Customers;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection was not found.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateCustomerRequestValidator>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
