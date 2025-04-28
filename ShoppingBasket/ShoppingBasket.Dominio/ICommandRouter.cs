namespace ShoppingBasket.Dominio;

public interface ICommandRouter
{
    public Task InvokeAsync<TCommand>(TCommand command) where TCommand : class; 
}