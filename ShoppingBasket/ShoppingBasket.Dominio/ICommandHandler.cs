namespace ShoppingBasket.Dominio;

public interface ICommandHandler<TCommand>
{
    void Handle(TCommand command);
}