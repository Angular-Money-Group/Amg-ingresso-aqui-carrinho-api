namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class CreditCardNotValidException : Exception
    {
        public CreditCardNotValidException()
        {
        }

        public CreditCardNotValidException(string message)
            : base(message)
        {
        }

        public CreditCardNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}