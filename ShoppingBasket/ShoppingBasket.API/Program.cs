using ShoppingBasket.API;
using ShoppingBasket.Dominio;
using ShoppingBasket.EventStore;
using Weasel.Core;
using Wolverine;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
var martenConnectionString = builder.Configuration.GetConnectionString("MartenEventStore") ??
                             throw new ArgumentNullException(
                                 "La cadena de conexión 'MartenEventStore' no está configurada.");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddHealthChecks()
    .AddNpgSql(martenConnectionString);

builder.UseWolverine(options =>
    {
        options.Services.AddMartenConfiguration(martenConnectionString,
            isDevelopment).IntegrateWithWolverine();
        options.Policies.AutoApplyTransactions();
        options.Durability.Mode = DurabilityMode.MediatorOnly;
    }
);

builder.Services.AddMartenEventStore();
builder.Services.AddScoped<ICommandRouter, WolverineCommandRouter>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseHealthChecks("/health");

//app.MapPost("/product", async (CreateProduct command, ICommandRouter router) =>
//{
//    await router.InvokeAsync(command);
//});

app.Run();

//public record CreateProduct(string Name, string Description, decimal Price);

//public record ProductCreated(string Name, string Description, decimal Price);

//public class CreateProductHandler(IEventStore eventStore) : ICommandHandler<CreateProduct>
//{
//    public Task HandleAsync(CreateProduct command)
//    {
//        var productId = Guid.NewGuid();
//        var @event = new ProductCreated(
//            command.Name,
//            command.Description,
//            command.Price
//        );
        
//        eventStore.AppendEvent(productId, @event);
        
//        return Task.CompletedTask;
//    }
//}