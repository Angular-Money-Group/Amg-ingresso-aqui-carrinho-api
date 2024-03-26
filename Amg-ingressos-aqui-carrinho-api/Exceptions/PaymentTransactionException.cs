namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class PaymentTransactionException : Exception
    {
        public PaymentTransactionException()
        {
        }

        public PaymentTransactionException(string message)
            : base(message)
        {
        }

        public PaymentTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}