namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class SaveTransactionException : Exception
    {

        public SaveTransactionException()
        {
        }

        public SaveTransactionException(string message)
            : base(message)
        {
        }

        public SaveTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}