namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class CreditCardNotValidExeption : Exception
    {

        public CreditCardNotValidExeption()
        {
        }

        public CreditCardNotValidExeption(string message)
            : base(message)
        {
        }

        public CreditCardNotValidExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}