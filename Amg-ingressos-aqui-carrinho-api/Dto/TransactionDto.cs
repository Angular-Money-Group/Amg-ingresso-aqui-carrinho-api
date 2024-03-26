using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionDto
    {
        public TransactionDto()
        {
            IdUser = string.Empty;
            IdEvent = string.Empty;
            TransactionItensDto = new List<TransactionItenDto>();
        }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public string IdUser { get; set; }

        /// <summary>
        /// Numero Total de Tickets
        /// </summary>
        public int TotalTicket { get; set; }

        /// <summary>
        /// Total Valor da transacao
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public string IdEvent { get; set; }

        /// <summary>
        /// TransactionItens
        /// </summary>
        public List<TransactionItenDto> TransactionItensDto { get; set; }

        public List<Transaction> DtoListToModelList(List<TransactionDto> listTransactionDto)
        {
            return listTransactionDto.Select(DtoToModel).ToList();
        }

        public Transaction DtoToModel()
        {
            return new Transaction()
            {
                IdPerson = this.IdUser,
                IdEvent = this.IdEvent,
                DateRegister = DateTime.Now,
                TotalTicket = this.TotalTicket,
                TotalValue = this.TotalValue
            };
        }

        public Transaction DtoToModel(TransactionDto transactionDto)
        {
            return new Transaction()
            {
                IdPerson = transactionDto.IdUser,
                IdEvent = transactionDto.IdEvent,
                DateRegister = DateTime.Now,
                TotalTicket = transactionDto.TotalTicket,
                TotalValue = transactionDto.TotalValue
            };
        }
    }
}