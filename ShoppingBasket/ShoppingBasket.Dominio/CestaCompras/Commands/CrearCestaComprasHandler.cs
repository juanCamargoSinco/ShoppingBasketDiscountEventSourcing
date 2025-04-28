
using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras.Commands;
public record CrearCestaCompras(Guid Id);

public class CrearCestaComprasHandler(IEventStore eventStore) : ICommandHandler<CrearCestaCompras>
{
    public void Handle(CrearCestaCompras command)
    {
        var cestaComprasCreada = new CestaComprasCreada(command.Id);

        eventStore.AppendEvent(command.Id, cestaComprasCreada);
    }
}