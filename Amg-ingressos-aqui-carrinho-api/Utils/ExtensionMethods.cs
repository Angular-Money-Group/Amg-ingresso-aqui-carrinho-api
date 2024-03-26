using Amg_ingressos_aqui_carrinho_api.Exceptions;

namespace Amg_ingressos_aqui_carrinho_api.Utils
{
    public static class ExtensionMethods
    {
        public static void ValidateIdMongo(this string idmongo, string model)
        {
            if (string.IsNullOrEmpty(idmongo))
                throw new IdMongoException($"{model} é obrigatório");
            if (idmongo.Length < 24)
                throw new IdMongoException($"{model} é obrigatório e está menor que 24 digitos");
        }
    }
}