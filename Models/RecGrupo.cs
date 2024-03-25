namespace server.Models
{
    public class RecGrupo : Base
    {
        public Guid IdProyecto { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public List<RiMenu> Menus { get; set; } = new List<RiMenu>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<RiAccesoModelo> Accesos { get; set; } = new List<RiAccesoModelo>();
        public required Proyecto Proyecto { get; set; }
    }
}
