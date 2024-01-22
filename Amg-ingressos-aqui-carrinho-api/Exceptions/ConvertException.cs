namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class ConvertException: Exception
    {

        public ConvertException()
        {
        }

        public ConvertException(string message)
            : base(message)
        {
        }

        public ConvertException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}