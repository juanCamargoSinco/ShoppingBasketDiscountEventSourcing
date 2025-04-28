using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras.Commands;
public record AumentarCantidadItem(Guid IdCesta, string Nombre, int Cantidad);
public class AumentarItemHandler(IEventStore eventStore) : ICommandHandler<AumentarCantidadItem>
{
    public void Handle(AumentarCantidadItem command)
    {
        if(command.Cantidad < 0)
            throw new InvalidOperationException("Cantidad invalida");

        var cesta = eventStore.GetAggregateRootAsync<CestaCompras>(command.IdCesta).GetAwaiter().GetResult();

        if (cesta.Items.Any(x => x.Nombre == command.Nombre) is false)
            throw new InvalidOperationException("Item inexistente");

        var itemAumentado = new ItemAumentado(command.IdCesta, command.Nombre, command.Cantidad);
        eventStore.AppendEvent(command.IdCesta, itemAumentado);
    }
}
