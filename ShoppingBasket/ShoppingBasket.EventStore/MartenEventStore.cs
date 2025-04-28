using ShoppingBasket.Dominio;
using Marten;

namespace ShoppingBasket.EventStore;

public class MartenEventStore(IDocumentSession session, IQuerySession querySession) : IEventStore
{
    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId)
        where TAggregateRoot : AggregateRoot, new()
    {
        return querySession.Events.AggregateStreamAsync<TAggregateRoot>(aggregateId);
    }

    public void AppendEvent(Guid aggregateId, object eventData)
    {
        session.Events.Append(aggregateId, eventData);
    }

    public Task SaveChangesAsync()
    {
        return session.SaveChangesAsync();
    }
}