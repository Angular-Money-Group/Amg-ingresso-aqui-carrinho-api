using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionCompletDto : Transaction
    {
        public TransactionCompletDto()
        {
            TransactionItens = new List<TransactionIten>();
        }

        public List<TransactionIten> TransactionItens { get; set; }

        public List<TransactionCompletDto> ModelListToDtoList(List<TransactionComplet> listTransaction)
        {
            return listTransaction.Select(ModelToDto).ToList();
        }

        public TransactionCompletDto ModelToDto(TransactionComplet transactionData)
        {
            return new TransactionCompletDto()
            {
                DateRegister = transactionData.DateRegister,
                Details = transactionData.Details,
                Discount = transactionData.Discount,
                Id = transactionData.Id,
                IdEvent = transactionData.IdEvent,
                IdPerson = transactionData.IdPerson,
                PaymentIdService = transactionData.PaymentIdService,
                PaymentMethod = transactionData.PaymentMethod,
                PaymentMethodPix = transactionData.PaymentMethodPix,
                PaymentPix = transactionData.PaymentPix,
                ReturnUrl = transactionData.ReturnUrl,
                Stage = transactionData.Stage,
                Status = transactionData.Status,
                Tax = transactionData.Tax,
                TotalTicket = transactionData.TotalTicket,
                TotalValue = transactionData.TotalValue,
                TransactionItens = transactionData.TransactionItens
            };
        }
    }
}