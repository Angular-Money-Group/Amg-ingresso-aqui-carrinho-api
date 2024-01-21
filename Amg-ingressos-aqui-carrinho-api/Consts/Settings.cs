namespace Amg_ingressos_aqui_carrinho_api.Consts
{
    public static class Settings
    {
        public readonly static string EmailServiceApi = "http://api.ingressosaqui.com:3011/";
        public readonly static string UriEmailTicket = "v1/notification/emailTicket";
        public readonly static string TicketServiceApi = "http://api.ingressosaqui.com/";
        public readonly static string UriTicketsLot = "v1/tickets/lote/";
        public readonly static string UriGetTicketDataUser = "v1/tickets/{0}/datauser";
        public readonly static string UriGetTicketDataEvent = "v1/tickets/{0}/dataevent";
        public readonly static string UriUpdateTicket = "v1/tickets/";
        public readonly static string QrCodeServiceApi = "http://api.ingressosaqui.com:3004/";
        public readonly static string UriGenerateQrCode = "v1/generate-qr-code?data=";
        public readonly static string HostImg = "https://api.ingressosaqui.com/imagens/";
    }
}