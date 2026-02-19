using Agdt.Core;
using Agdt.Host.Helpers;
using Agdt.Host.Middleware;
using Agdt.Host.Services;
using Agdt.Persistent;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<LocalizationService>();
builder.Services.AddPersistent(connectionString);
builder.Services.AddMediatR(cfg => {
	cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddCore();

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.MapOptions(JsonHelper.DefaultJsonOptions);
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Patient API",
		Version = "v1",
		Description = "Patient API"
	});

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

await app.Services.InitDbAsync();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("v1/swagger.json", "Patient API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
