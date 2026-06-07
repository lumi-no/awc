using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Middleware;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration["DATABASE_URL"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IValidator<CreateTaskDto>, CreateTaskValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskDto>, UpdateTaskValidator>();
builder.Services.AddScoped<IValidator<TaskQueryDto>, TaskQueryValidator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        string frontendOrigin = builder.Configuration["FRONTEND_ORIGIN"] ?? "*";

        policy
            .WithOrigins(frontendOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestIdMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FrontendPolicy");

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
