namespace Amg_ingressos_aqui_carrinho_api.Consts
{
    public static class Settings
    {
        //Notification Service
        public readonly static string EmailServiceApi = "http://api.ingressosaqui.com:3011/";
        public readonly static string UriEmailTicket = "v1/notification/emailTicket";
        //ticket Service
        public readonly static string TicketServiceApi = "http://api.ingressosaqui.com/";
        public readonly static string UriTicketsLot = "v1/tickets/lote/";
        public readonly static string UriGetTicketDataUser = "v1/tickets/{0}/datauser";
        public readonly static string UriGetTicketDataEvent = "v1/tickets/{0}/dataevent";
        public readonly static string UriUpdateTicket = "v1/tickets/";
        //QrCodeService
        public readonly static string QrCodeServiceApi = "http://api.ingressosaqui.com:3004/";
        public readonly static string UriGenerateQrCode = "v1/generate-qr-code?data=";
        //User Service 
        public readonly static string UserServiceApi = "http://api.ingressosaqui.com:3005/";
        public readonly static string UriGetUser = "v1/user/";
        //host img
        public readonly static string HostImg = "https://api.ingressosaqui.com/imagens/";
        //cielo
        public readonly static string CieloZeroAuth = "https://apisandbox.cieloecommerce.cielo.com.br/1/zeroauth";
        public readonly static string CieloStatusPayment = "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/";

        //Pagbank
        public readonly static string PagbankStatusPayment = "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/";
        
        
        
    }
}