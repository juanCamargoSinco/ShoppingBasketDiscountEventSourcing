using ShoppingBasket.Dominio.CestaCompras.Commands;
using ShoppingBasket.Dominio.CestaCompras.Events;
using D = ShoppingBasket.Dominio.CestaCompras;

namespace ShoppingBasket.Dominio.Tests.CestaCompras;
public class AumentarCantidadItemSpecifications : CommandHandlerTest<AumentarCantidadItem>
{
    protected override ICommandHandler<AumentarCantidadItem> Handler => new AumentarItemHandler(eventStore);


    [Fact]
    public void Si_AumentoCantidadDeItemInexistenteEnCestaCompras_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId));

        var aumentarCantidad = () => When(new AumentarCantidadItem(_aggregateId, "Item A", 1));
        var excepcion = Assert.Throws<InvalidOperationException>(aumentarCantidad);
        Assert.Equal("Item inexistente", excepcion.Message);
    }

    [Fact]
    public void Si_AumentoCantidadDeItemExistenteEnCestaCompras_Debe_EmitirEventoItemAumentado_Y_AumentarCantidadItem()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 1, 10));

        When(new AumentarCantidadItem(_aggregateId, "Item A", 4));

        Then(new ItemAumentado(_aggregateId, "Item A", 4));
        And<D.CestaCompras>(cesta => cesta.ObtenerCantidadItem("Item A"), 5);
     }

    [Fact]
    public void Si_AumentoCantidadItemExistenteEnCestaCompras_Debe_EmitirEventoItemAumentado_Y_ItemNoRepetirseEnLaCesta()
    {
        Given(new CestaComprasCreada(_aggregateId)
            , new ItemAñadido(_aggregateId, "Item A", 1, 10)
            , new ItemAumentado(_aggregateId, "Item A", 1));

        When(new AumentarCantidadItem(_aggregateId, "Item A", 4));

        Then(new ItemAumentado(_aggregateId, "Item A", 4));
        And<D.CestaCompras>(cesta => cesta.Items.Where(x => x.Nombre == "Item A").Count(), 1);
        And<D.CestaCompras>(cesta => cesta.ObtenerCantidadItem("Item A"), 6);
    }

    [Fact]
    public void Si_AumentoCantidadItemConCantidadNegativa_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 1, 10));

        var añadirItem = () => When(new AumentarCantidadItem(_aggregateId, "Item A", -1));
        var excepcion = Assert.Throws<InvalidOperationException>(añadirItem);
        Assert.Equal("Cantidad invalida", excepcion.Message);
    }


}

