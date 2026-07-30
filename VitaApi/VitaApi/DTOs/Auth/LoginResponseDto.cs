namespace VitaApi.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";

        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";

        public string? Especialidade { get; set; }
    }
}
