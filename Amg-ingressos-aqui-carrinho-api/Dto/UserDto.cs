namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class UserDto
    {
        public UserDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            Cpf = string.Empty;
            Email = string.Empty;
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}