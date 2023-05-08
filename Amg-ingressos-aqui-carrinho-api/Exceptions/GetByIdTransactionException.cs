namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class GetByIdTransactionException : Exception
    {

        public GetByIdTransactionException()
        {
        }

        public GetByIdTransactionException(string message)
            : base(message)
        {
        }

        public GetByIdTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}