
using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras.Commands;
public record AñadirItem(Guid IdCesta, string Nombre, int Cantidad, decimal Precio);
public class AñadirItemHandler(IEventStore eventStore) : ICommandHandler<AñadirItem>
{
    public void Handle(AñadirItem command)
    {
        ValidarItem(command);

        var cestaCompras = eventStore.GetAggregateRootAsync<CestaCompras>(command.IdCesta).GetAwaiter().GetResult();

        var (IdCesta, Nombre, Cantidad, Precio) = command;
        if (cestaCompras.Items.Any(x => x.Nombre == Nombre))
            throw new InvalidOperationException("Producto ya existente");

        var itemAñadido = new ItemAñadido(IdCesta, Nombre, Cantidad, command.Precio);

        eventStore.AppendEvent(IdCesta, itemAñadido);

    }

    private static void ValidarItem(AñadirItem command)
    {
        if (command.Cantidad < 0)
            throw new InvalidOperationException("Cantidad invalida");

        if (command.Precio < 0)
            throw new InvalidOperationException("Precio invalido");
    }
}
