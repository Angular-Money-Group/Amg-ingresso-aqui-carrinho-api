namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class EventDto
    {
        public EventDto()
        {
            id = string.Empty;
            Name = string.Empty;
            Local = string.Empty;
            Type = string.Empty;
            Image = string.Empty;
            Address = new Model.Address();
        }

        public string id { get; set; }
        public string Name { get; set; }
        public string Local { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Enum.StatusEvent Status { get; set; }
        public Model.Address Address { get; set; }
    }
}