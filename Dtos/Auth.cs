namespace server.Dtos
{
    public class RegisterDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string ProjectCluster { get; set; }
    }
    public class LoginDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string ProjectCluster { get; set; }
    }
    public class CambiarContraDTO
    {
        public required string Actual { get; set; }
        public required string New { get; set; }
        public required string Confirm { get; set; }
    }
    public class CambiarCorreoDTO
    {
        public required string Password { get; set; }
        public required string NewEmail { get; set; }
    }
}