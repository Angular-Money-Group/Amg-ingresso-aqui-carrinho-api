namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class GetByPersonTransactionException : Exception
    {

        public GetByPersonTransactionException()
        {
        }

        public GetByPersonTransactionException(string message)
            : base(message)
        {
        }

        public GetByPersonTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}