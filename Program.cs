using Microsoft.OpenApi.Models;

// Add services to the container.
var builder = WebApplication.CreateBuilder (args);
builder.Services.AddControllers ();
builder.Services.AddEndpointsApiExplorer ();
builder.Services.AddSwaggerGen (c =>
{
    c.SwaggerDoc ("v1", new OpenApiInfo { Title = "Car Stock API", Version = "v1" });
});
var app = builder.Build ();

// Configure the HTTP request pipeline.
app.UseSwagger ();
app.UseSwaggerUI (c =>
{
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Car Stock API V1");
});
app.UseHttpsRedirection ();
app.MapControllers ();

app.Run ();
