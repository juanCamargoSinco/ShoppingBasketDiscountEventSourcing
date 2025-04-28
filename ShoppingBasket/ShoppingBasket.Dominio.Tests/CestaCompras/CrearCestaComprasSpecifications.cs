using ShoppingBasket.Dominio.CestaCompras.Commands;
using ShoppingBasket.Dominio.CestaCompras.Events;
using D = ShoppingBasket.Dominio.CestaCompras;

namespace ShoppingBasket.Dominio.Tests.Basket;


public class CrearCestaComprasSpecifications : CommandHandlerTest<CrearCestaCompras>
{
    protected override ICommandHandler<CrearCestaCompras> Handler => new CrearCestaComprasHandler(eventStore);

    [Fact]
    public void Si_CreoCestaCompras_Debe_EmitirEventoCestaComprasCreada()
    {

        When(new CrearCestaCompras(_aggregateId));

        Then(new CestaComprasCreada(_aggregateId));
        And<D.CestaCompras>(cesta => cesta.Id, _aggregateId);
    }
}

