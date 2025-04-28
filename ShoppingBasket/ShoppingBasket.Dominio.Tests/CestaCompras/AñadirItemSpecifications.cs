using D =  ShoppingBasket.Dominio.CestaCompras;
using ShoppingBasket.Dominio.CestaCompras.Commands;
using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.Tests.CestaCompras;
public class AñadirItemSpecifications : CommandHandlerTest<AñadirItem>
{
    protected override ICommandHandler<AñadirItem> Handler => new AñadirItemHandler(eventStore);

    [Fact]
    public void Si_AñadoItemACestaCompras_Debe_EmitirEventoItemAñadido_Y_IncluirItemEnCestaCompras()
    {
        Given(new CestaComprasCreada(_aggregateId));

        When(new AñadirItem(_aggregateId, "Item A", 1, 10));

        Then(new ItemAñadido(_aggregateId, "Item A", 1, 10));
        And<D.CestaCompras>(cesta => cesta.Items.Any(x => x.Nombre == "Item A"), true);

    }

    [Fact]
    public void Si_AñadoItemExistenteACestaCompras_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 1, 10));

        var añadirItem = () => When(new AñadirItem(_aggregateId, "Item A", 1, 10));
        var excepcion = Assert.Throws<InvalidOperationException>(añadirItem);
        Assert.Equal("Producto ya existente", excepcion.Message);
    }

    [Fact]
    public void Si_AñadoItemConCantidadNegativa_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId));

        var añadirItem = () => When(new AñadirItem(_aggregateId, "Item A", -1, 10));
        var excepcion = Assert.Throws<InvalidOperationException>(añadirItem);
        Assert.Equal("Cantidad invalida", excepcion.Message);
    }

    [Fact]
    public void Si_AñadoItemConPrecioNegativo_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 1, 10));

        var añadirItem = () => When(new AñadirItem(_aggregateId, "Item A", 1, -10));
        var excepcion = Assert.Throws<InvalidOperationException>(añadirItem);
        Assert.Equal("Precio invalido", excepcion.Message);
    }

    //Total

    [Fact]
    public void Si_AñadoItem_Debe_SubTotalCestaSerPrecioItemPorCantidad()
    {
        Given(new CestaComprasCreada(_aggregateId));

        When(new AñadirItem(_aggregateId, "Item A", 5, 10));

        Then(new ItemAñadido(_aggregateId, "Item A", 5, 10));
        And<D.CestaCompras>(cesta => cesta.SubTotal, 50);
    }

    [Fact]
    public void Si_ValorTotalItemAñadidoEsMayorA100_Debe_AplicarDescuentoDel5Porciento()
    {
        Given(new CestaComprasCreada(_aggregateId));

        When(new AñadirItem(_aggregateId, "Item A", 12, 10));

        Then(new ItemAñadido(_aggregateId, "Item A", 12, 10));
        And<D.CestaCompras>(cesta => cesta.SubTotal, 120);
        //And<D.CestaCompras>(cesta => cesta.Total, 114);
        And<D.CestaCompras>(cesta => cesta.CalcularPrecioTotalCesta(), 114);
    }

    [Fact]
    public void Si_ValorTotalItemAñadidoEsMayorA200_Debe_AplicarDescuentoDel10Porciento()
    {
        Given(new CestaComprasCreada(_aggregateId));

        When(new AñadirItem(_aggregateId, "Item A", 23, 10));

        Then(new ItemAñadido(_aggregateId, "Item A", 23, 10));
        And<D.CestaCompras>(cesta => cesta.SubTotal, 230);
        //And<D.CestaCompras>(cesta => cesta.Total, 207);
        And<D.CestaCompras>(cesta => cesta.CalcularPrecioTotalCesta(), 207);
    }

    [Fact]
    public void Si_ValorTotalItemAñadidoEs200_Debe_AplicarDescuentoDel5Porciento()
    {
        Given(new CestaComprasCreada(_aggregateId));

        When(new AñadirItem(_aggregateId, "Item A", 20, 10));

        Then(new ItemAñadido(_aggregateId, "Item A", 20, 10));
        And<D.CestaCompras>(cesta => cesta.SubTotal, 200);
        //And<D.CestaCompras>(cesta => cesta.Total, 190);
        And<D.CestaCompras>(cesta => cesta.CalcularPrecioTotalCesta(), 190);
    }

}



