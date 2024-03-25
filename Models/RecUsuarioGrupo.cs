namespace server.Models
{
    public class RecUsuarioGrupo : Base
    {
        public Guid IdUsuario { get; set; }
        public Guid IdGrupo { get; set; }
        public required RecGrupo Grupo { get; set; }
        public required Usuario Usuario { get; set; }
    }
}
