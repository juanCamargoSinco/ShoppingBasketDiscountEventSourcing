using Marten;
using Marten.Events;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using IEventStore = ShoppingBasket.Dominio.IEventStore;

namespace ShoppingBasket.EventStore;

public static class MartenEventStoreExtensions
{
    public static MartenServiceCollectionExtensions.MartenConfigurationExpression AddMartenConfiguration(this IServiceCollection service, string connectionString,
        bool isDevelopment)
    {
        return service.AddMarten(options => 
        {
            options.Connection(connectionString);
            options.UseSystemTextJsonForSerialization();
            options.Events.StreamIdentity = StreamIdentity.AsGuid;

            if (isDevelopment)
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
    
        }).UseLightweightSessions();
    }
    
    public static void AddMartenEventStore(this IServiceCollection service)
    {
        service.AddScoped<IEventStore, MartenEventStore>();
    }
}