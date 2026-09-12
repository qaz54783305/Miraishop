namespace MiraiShop.Domain.Exceptions;

public class CartException : Exception
{
    public CartException(string message) : base(message)
    {
    }
}