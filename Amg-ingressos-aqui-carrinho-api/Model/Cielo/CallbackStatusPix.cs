namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback
{

    public class PaymentTransactionGetStatus
    {
        public PaymentTransactionGetStatus()
        {
            MerchantOrderId = string.Empty;
            AcquirerOrderId = string.Empty;
            Customer = new CustomerGetStatus();
            Payment = new PaymentGetStatus();
        }

        public string MerchantOrderId { get; set; }
        public string AcquirerOrderId { get; set; }
        public CustomerGetStatus Customer { get; set; }
        public PaymentGetStatus Payment { get; set; }
    }
}