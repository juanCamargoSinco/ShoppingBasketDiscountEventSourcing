namespace ShoppingBasket.Dominio.CestaCompras.Events;
public record ItemAñadido(Guid IdCesta, string Nombre, int Cantidad, decimal Precio);

