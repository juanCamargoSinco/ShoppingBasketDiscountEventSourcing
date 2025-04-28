using ShoppingBasket.Dominio.CestaCompras.Commands;
using ShoppingBasket.Dominio.CestaCompras.Events;
using D = ShoppingBasket.Dominio.CestaCompras;

namespace ShoppingBasket.Dominio.Tests.CestaCompras;
public class ReducirCantidadItemSpecifications : CommandHandlerTest<ReducirCantidadItem>
{
    protected override ICommandHandler<ReducirCantidadItem> Handler => new ReducirCantidadItemHandler(eventStore);


    [Fact]
    public void Si_ReduzcoCantidadDeItemInexistenteEnCestaCompras_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId));

        var reducirCantidad = () => When(new ReducirCantidadItem(_aggregateId, "Item A", 1));
        var excepcion = Assert.Throws<InvalidOperationException>(reducirCantidad);
        Assert.Equal("Item inexistente", excepcion.Message);
    }

    [Fact]
    public void Si_ReduzcoMayorCantidadExistenteDeItemEnCesta_Debe_LanzarExcepcion()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 2, 10));

        var reducirCantidad = () => When(new ReducirCantidadItem(_aggregateId, "Item A", 3));
        var excepcion = Assert.Throws<InvalidOperationException>(reducirCantidad);
        Assert.Equal("Cantidad invalida", excepcion.Message);
    }

    [Fact]
    public void Si_ReduzcoCantidadDeItem_Debe_EmitirEventoItemReducido_Y_ReducirCantidadItemEnCesta()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 5, 10));

        When(new ReducirCantidadItem(_aggregateId, "Item A", 3));

        Then(new ItemReducido(_aggregateId, "Item A", 3));
        And<D.CestaCompras>(cesta => cesta.ObtenerCantidadItem("Item A"), 2);
    }

    [Fact]
    public void Si_ReduzcoCantidadDeItemACero_Debe_EmitirEventoItemEliminado_Y_EliminarItemDeCesta()
    {
        Given(new CestaComprasCreada(_aggregateId), new ItemAñadido(_aggregateId, "Item A", 3, 10));

        When(new ReducirCantidadItem(_aggregateId, "Item A", 3));

        Then(new ItemEliminado(_aggregateId, "Item A"));
        And<D.CestaCompras>(cesta => cesta.Items.Any(x => x.Nombre == "Item A"), false);
    }
}