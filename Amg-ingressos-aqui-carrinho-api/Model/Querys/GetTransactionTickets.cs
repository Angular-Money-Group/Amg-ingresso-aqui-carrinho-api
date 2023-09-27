using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Querys.GetTransactionTicket
{
    public class GetTransactionTickets
    {
        
        public string _id { get; set; }
        public string IdPerson { get; set; }
        public string IdEvent { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Tax { get; set; }
        public string TotalValue { get; set; }
        public string Discount { get; set; }
        public int Status { get; set; }
        public int Stage { get; set; }
        public object ReturnUrl { get; set; }
        public string PaymentIdService { get; set; }
        public string Details { get; set; }
        public int TotalTicket { get; set; }
        public DateTime DateRegister { get; set; }
        public List<TransactionIten> TransactionIten { get; set; }
    
    }
    public class PaymentMethod
    {
        public object IdPaymentMethod { get; set; }
        public int TypePayment { get; set; }
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool SaveCard { get; set; }
        public string Brand { get; set; }
        public int Installments { get; set; }
    }

    public class Ticket
    {
        public string _id { get; set; }
        public string IdLot { get; set; }
        public string IdUser { get; set; }
        public object Position { get; set; }
        public string Value { get; set; }
        public bool isSold { get; set; }
        public object Status { get; set; }
        public object IdColab { get; set; }
        public bool ReqDocs { get; set; }
        public string QrCode { get; set; }
    }

    public class TransactionIten
    {
        public string _id { get; set; }
        public string IdTransaction { get; set; }
        public string IdTicket { get; set; }
        public bool HalfPrice { get; set; }
        public string TicketPrice { get; set; }
        public string Details { get; set; }
        public List<Ticket> ticket { get; set; }
    }


}