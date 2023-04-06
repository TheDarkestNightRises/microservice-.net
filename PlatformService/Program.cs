using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
         opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
