namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class UpdateTransactionException : Exception
    {

        public UpdateTransactionException()
        {
        }

        public UpdateTransactionException(string message)
            : base(message)
        {
        }

        public UpdateTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}