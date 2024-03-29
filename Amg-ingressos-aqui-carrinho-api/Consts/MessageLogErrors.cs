namespace Amg_ingressos_aqui_carrinho_api.Consts
{
    public static class MessageLogErrors
    {
        public const string saveTransactionMessage = "SaveTransactionAsync : Erro inesperado ao salvar uma transação";
        public const string paymentTransactionMessage = "PaymentTransactionAsync : Erro inesperado ao realizar pagamento de uma transação";
        public const string updateTransactionMessage = "UpdateTransactionAsync : Erro inesperado ao atualizar uma transação";
        public const string getByIdTransactionMessage = "GetByIdTransactionAsync : Erro inesperado ao buscar uma transação";
        public const string getByPersonTransactionMessage = "GetByPersonTransactionAsync : Erro inesperado ao buscar uma transação";
        public const string Process = "{0}:{1} - erro ao processar transação.";
        public const string Get = "{0}:{1} - erro ao buscar transação.";
    }
}