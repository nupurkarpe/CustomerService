using CustomerService.Application.Interface;
using CustomerService.Application.Mapping;
using CustomerService.Infrastructure.Data;
using CustomerService.Infrastructure.Repository;
using CustomerService.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthAddress"] ?? "loanauth-fxafbnechyf7e2dm.canadacentral-01.azurewebsites.net");
});


builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IDocTypeRepo, DocTypeRepo>();
builder.Services.AddScoped<IKycRepo, KycRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
