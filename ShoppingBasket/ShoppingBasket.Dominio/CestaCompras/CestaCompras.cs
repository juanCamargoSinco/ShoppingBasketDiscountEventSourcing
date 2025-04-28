using ShoppingBasket.Dominio.CestaCompras.Events;

namespace ShoppingBasket.Dominio.CestaCompras;
public class CestaCompras : AggregateRoot
{
    public Guid Id { get; private set; }

    private readonly List<Item> _items = [];
    public decimal SubTotal => Items.Sum(x => x.PrecioTotal);
    //public decimal Total => SubTotal - (SubTotal * ObtenerTasaDescuento());

    public IReadOnlyList<Item> Items => _items;


    public void Apply(CestaComprasCreada @event)
    {
        Id = @event.Id;
    }

    public void Apply(ItemAñadido @event)
    {
        _items.Add(new Item(@event.Nombre, @event.Cantidad, @event.Precio));
    }
    public void Apply(ItemAumentado @event)
    {
        _items.Where(x => x.Nombre == @event.Nombre).First().AumentarCantidad(@event.Cantidad);
    }
    public void Apply(ItemEliminado @event)
    {
        int index = _items.FindIndex(x => x.Nombre == @event.Nombre);
        //if (index != -1) se da por sentado que el handler valido que el item exista, por tanto el index se deberia encontrar
            _items.RemoveAt(index);
    }
    public void Apply(ItemReducido @event)
    {
        _items.Where(x => x.Nombre == @event.Nombre).First().ReducirCantidad(@event.Cantidad);
    }

    public int ObtenerCantidadItem(string producto)
    {
        return _items.Where(x => x.Nombre == producto).FirstOrDefault()?.Cantidad ?? 0;
    }
    public decimal CalcularPrecioTotalCesta()
    {
        var tasaDescuento = ObtenerTasaDescuento();
        var valorDescuento = SubTotal * tasaDescuento;
        var total = SubTotal - valorDescuento;
        return total;
    }

    private decimal ObtenerTasaDescuento()
    {
        return SubTotal switch
        {
            > 200 => 0.10m,
            > 100 => 0.05m,
            _ => 0
        };
    }
}