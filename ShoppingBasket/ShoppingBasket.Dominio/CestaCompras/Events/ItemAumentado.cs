namespace ShoppingBasket.Dominio.CestaCompras.Events;
public record ItemAumentado(Guid IdCesta, string Nombre, int Cantidad);