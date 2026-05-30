using Microsoft.EntityFrameworkCore;
using PJAKT_APBD_Cw8.Services;
using PJAKT_APBD_Cw8.Infrastructure;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IDbService, DbService>();

builder.Services.AddDbContext<HospitalContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Api"));
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();