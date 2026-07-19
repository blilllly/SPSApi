using SPSApi.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddModules(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  await app.Services.MigrateModulesAsync();
  app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}

app.MapModules();

app.Run();