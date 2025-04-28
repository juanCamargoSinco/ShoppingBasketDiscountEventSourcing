using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras.Commands;

public record EliminarItem(Guid IdCesta, string Nombre);
public class EliminarItemHandler(IEventStore eventStore) : ICommandHandler<EliminarItem>
{
    public void Handle(EliminarItem command)
    {
        var cesta = eventStore.GetAggregateRootAsync<CestaCompras>(command.IdCesta).GetAwaiter().GetResult();

        if (cesta.Items.Any(x => x.Nombre == command.Nombre) == false)
            throw new InvalidOperationException("El item a eliminar no se encuentra en la cesta");

        var itemEliminado = new ItemEliminado(command.IdCesta, command.Nombre);

        eventStore.AppendEvent(command.IdCesta, itemEliminado);
    }
}
