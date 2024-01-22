namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback
{
    public class CallbackPix
    {
        public CallbackPix()
        {
            MerchantOrderId = string.Empty;
            Customer = new Customer();
            Payment = new PaymentPix();
        }

        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public PaymentPix Payment { get; set; }
    }
}