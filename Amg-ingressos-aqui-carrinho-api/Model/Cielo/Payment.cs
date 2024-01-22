using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class Payment
    {
        public Payment()
        {
            Type = string.Empty;
            Provider = string.Empty;
            Address = string.Empty;
            BoletoNumber = string.Empty;
            Assignor = string.Empty;
            Demonstrative = string.Empty;
            ExpirationDate = string.Empty;
            Identification = string.Empty;
            Instructions = string.Empty;
            ReturnUrl = string.Empty;
            SoftDescriptor = string.Empty;
            DebitCard = new DebitCardCielo();
            CreditCard = new CreditCardCielo();
        }

        [JsonProperty("Type")]
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonProperty("StAmountate")]
        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonProperty("Provider")]
        [JsonPropertyName("Provider")]
        public string Provider { get; set; }

        [JsonProperty("Address")]
        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonProperty("BoletoNumber")]
        [JsonPropertyName("BoletoNumber")]
        public string BoletoNumber { get; set; }

        [JsonProperty("Assignor")]
        [JsonPropertyName("Assignor")]
        public string Assignor { get; set; }

        [JsonProperty("Demonstrative")]
        [JsonPropertyName("Demonstrative")]
        public string Demonstrative { get; set; }

        [JsonProperty("ExpirationDate")]
        [JsonPropertyName("ExpirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("Identification")]
        [JsonPropertyName("Identification")]
        public string Identification { get; set; }

        [JsonProperty("Instructions")]
        [JsonPropertyName("Instructions")]
        public string Instructions { get; set; }

        [JsonProperty("ReturnUrl")]
        [JsonPropertyName("ReturnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("DebitCard")]
        [JsonPropertyName("DebitCard")]
        public DebitCardCielo DebitCard { get; set; }

        [JsonProperty("CreditCard")]
        [JsonPropertyName("CreditCard")]
        public CreditCardCielo CreditCard { get; set; }

        [JsonProperty("Installments")]
        [JsonPropertyName("Installments")]
        public int Installments { get; set; }

        [JsonProperty("SoftDescriptor")]
        [JsonPropertyName("SoftDescriptor")]
        public string SoftDescriptor { get; set; }
    }
}