namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class GetException : Exception
    {
        public GetException()
        {
        }

        public GetException(string message)
            : base(message)
        {
        }

        public GetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}