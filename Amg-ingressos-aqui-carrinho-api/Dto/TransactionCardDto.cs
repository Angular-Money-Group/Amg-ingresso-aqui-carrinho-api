using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionCardDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("month")]
        public string Month { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        public TransactionCardDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            Day = string.Empty;
            Month = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            Year = string.Empty;
        }
        
        public IEnumerable<TransactionCardDto> ModelListToDtoList(IEnumerable<TransactionComplet> listTransaction)
        {

            var listEvents = listTransaction.Select(e => e.Events.FirstOrDefault())
            .GroupBy(t => t?._Id).Select(c => new TransactionCardDto()
            {
                Name = c?.FirstOrDefault()?.Name ?? string.Empty,
                Id = c?.FirstOrDefault()?._Id ?? string.Empty,
                Day = c?.FirstOrDefault()?.StartDate.Day.ToString() ?? string.Empty,
                Month = c?.FirstOrDefault()?.StartDate.Month.ToString() ?? string.Empty,
                Year = c?.FirstOrDefault()?.StartDate.Year.ToString() ?? string.Empty,
                City = c?.FirstOrDefault()?.Address?.City ?? string.Empty,
                State = c?.FirstOrDefault()?.Address?.State ?? string.Empty,
                Description = c?.FirstOrDefault()?.Description ?? string.Empty,
                Image = c?.FirstOrDefault()?.Image ?? string.Empty
            });
            return listEvents;
        }
    }
}