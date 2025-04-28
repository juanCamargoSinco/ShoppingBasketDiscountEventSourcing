using D = ShoppingBasket.Dominio.CestaCompras;
using ShoppingBasket.Dominio.CestaCompras.Commands;
using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.Tests.CestaCompras;
public class EliminarItemSpecifications : CommandHandlerTest<EliminarItem>
{
    protected override ICommandHandler<EliminarItem> Handler => new EliminarItemHandler(eventStore);

    [Fact]
    public void Si_EliminoItemNoExistenteEnLaCestaCompras_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId));

        var eliminarItem = () => When(new EliminarItem(_aggregateId, "Item A"));
        var exception = Assert.Throws<InvalidOperationException>(eliminarItem);
        Assert.Equal("El item a eliminar no se encuentra en la cesta", exception.Message);
    }

    [Fact]
    public void Si_EliminoItemExistenteEnLaCestaCompras_Debe_EmitirEventoItemEliminado_Y_ItemDesaparecerDeCesta()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 2, 10));

        When(new EliminarItem(_aggregateId, "Item A"));

        Then(new ItemEliminado(_aggregateId, "Item A"));
        And<D.CestaCompras>(cesta => cesta.Items.Any(x => x.Nombre == "Item A"), false);
    }
}

