namespace ShoppingBasket.Dominio;

public interface IEventStore
{
    void AppendEvent(Guid aggregateId, object eventData);
    Task SaveChangesAsync();
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot,  new();
}