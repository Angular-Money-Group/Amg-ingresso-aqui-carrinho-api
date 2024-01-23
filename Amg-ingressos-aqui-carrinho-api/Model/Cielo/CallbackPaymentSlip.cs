namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback.Slip
{
    public class CallbackPaymentSlip
    {
        public CallbackPaymentSlip()
        {
            MerchantOrderId = string.Empty;
            Customer = new Customer();
            Payment = new PaymentSlip();
        }

        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public PaymentSlip Payment { get; set; }
    }
}