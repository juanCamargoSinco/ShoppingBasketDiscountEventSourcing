using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras.Commands;
public record ReducirCantidadItem(Guid IdCesta, string Nombre, int Cantidad);

public class ReducirCantidadItemHandler(IEventStore eventStore) : ICommandHandler<ReducirCantidadItem>
{
    public void Handle(ReducirCantidadItem command)
    {
        var cesta = eventStore.GetAggregateRootAsync<CestaCompras>(command.IdCesta).GetAwaiter().GetResult();

        var item = cesta.Items.FirstOrDefault(x => x.Nombre == command.Nombre);

        if (item is null)
            throw new InvalidOperationException("Item inexistente");

        if (item.Cantidad < command.Cantidad)
            throw new InvalidOperationException("Cantidad invalida");

        if (item.Cantidad - command.Cantidad == 0)
        {
            var itemEliminado = new ItemEliminado(command.IdCesta, command.Nombre);

            eventStore.AppendEvent(command.IdCesta, itemEliminado);
        }
        else
        {
            var itemReducido = new ItemReducido(command.IdCesta, command.Nombre, command.Cantidad);

            eventStore.AppendEvent(command.IdCesta, itemReducido);
        }

    }

}