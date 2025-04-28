namespace ShoppingBasket.Dominio.CestaCompras;
public class Item(string nombre, int cantidad, decimal precio)
{
    public string Nombre { get; } = nombre;
    public int Cantidad { get; private set; } = cantidad;
    public decimal PrecioUnitario { get; } = precio;
    public decimal PrecioTotal => PrecioUnitario * Cantidad;

    public void AumentarCantidad(int cantidad)
    {
        Cantidad += cantidad;
    }
    public void ReducirCantidad(int cantidad)
    {
        Cantidad -= cantidad;
    }
}