using PubSub.Application.Common;
using PubSub.Application.Events;
using PubSub.Domain.Events;
using PubSub.Infrastructure.EventBus;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<EventSubscriptionManager>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

builder.Services.AddTransient<UserCreatedEventHandler>();
builder.Services.AddTransient<OrderCreatedEventHandler>();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<UserCreatedEvent, UserCreatedEventHandler>();
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
