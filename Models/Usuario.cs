

namespace server.Models
{
    public class Usuario : Base
    {
        public Guid IdProyecto { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public string? Telefono { get; set; }
        public bool? Verificado { get; set; }
        public string? CodigoSecreto { get; set; }
        public string? Firma { get; set; }
        public string? Notificacion { get; set; }
        public required Proyecto Proyecto { get; set; } = null!;
        public List<RecGrupo> Grupos { get; set; } = new List<RecGrupo>();
    }
}