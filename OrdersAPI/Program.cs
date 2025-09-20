using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrdersAPI.Data;
using OrdersAPI.Dtos;
using OrdersAPI.Features.Commands.Create;
using OrdersAPI.Features.Queries.GetById;
using OrdersAPI.Features.Queries.List;
using OrdersAPI.Models;
using OrdersAPI.Projections;
using OrdersAPI.Services;
using OrdersAPI.Services.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ensure SQLite DBs live under Data/DB at the project root
var contentRoot = builder.Environment.ContentRootPath;
var dbDirectory = Path.Combine(contentRoot, "Data", "DB");
Directory.CreateDirectory(dbDirectory);

var writeDbPath = Path.Combine(dbDirectory, "WriteDb.db");
var readDbPath = Path.Combine(dbDirectory, "ReadDb.db");

builder.Services.AddDbContext<WriteDbContext>(opt => opt.UseSqlite($"Data Source={writeDbPath}"));
builder.Services.AddDbContext<ReadDbContext>(opt => opt.UseSqlite($"Data Source={readDbPath}"));

builder.Services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
builder.Services.AddSingleton<IEventPublisher, InProcessEventPublisher>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Apply migrations for all DbContexts at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var writeDb = services.GetRequiredService<WriteDbContext>();
    var readDb = services.GetRequiredService<ReadDbContext>();

    writeDb.Database.Migrate();
    readDb.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Endpoints

app.MapPost("/api/orders", async (IMediator mediator, CreateOrderCommand command) =>
{
    try
    {
        var result = await mediator.Send(command);

        if (result == null)
            return Results.BadRequest("Failed To Create New Order");

        return Results.Created($"/api/order/{result?.Id}", result);
    }
    catch (ValidationException ex)
    {
        var errors = ex.Errors.Select(a => new { a.PropertyName, a.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapGet("/api/order/{Id}", async (IMediator mediator, int Id) =>
{

    var order = await mediator.Send(new GetOrderByIdQuery(Id));

    if (order == null)
        return Results.NotFound();

    return Results.Ok(order);
});

app.MapGet("/api/orders/list", async (IMediator mediator) =>
{
    var orders = await mediator.Send(new GetOrdersListQuery());

    if (orders == null || !orders.Any())
        return Results.NotFound();

    return Results.Ok(orders);
});

app.Run();
