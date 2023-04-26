using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Clients;
using SampleWebAPI.Data;
using SampleWebAPI.Kafka;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GravityAccelerationContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<GravityAccelerationClient>(c => c.BaseAddress = new Uri("http://localhost:5433/"));
builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaProducer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var dataContext = scope.ServiceProvider.GetRequiredService<GravityAccelerationContext>())
{
	dataContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
