namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class SaveException : Exception
    {
        public SaveException()
        {
        }

        public SaveException(string message)
            : base(message)
        {
        }

        public SaveException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}