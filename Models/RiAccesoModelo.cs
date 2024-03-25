namespace server.Models
{
    public class RiAccesoModelo : Base
    {
        public Guid IdGrupo { get; set; }
        public Guid IdModelo { get; set; }
        public required bool Ver { get; set; }
        public required bool Crear { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }
        public required RecGrupo Grupo { get; set; }
        public required RiModelo Modelo { get; set; }

    }
}
