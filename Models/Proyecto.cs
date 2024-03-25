using server.Constants;

namespace server.Models
{
    public class Proyecto : Base
    {
        public required string Nombre { get; set; }
        public required string AuthVersion {get;set;}
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<RiMenu> RiMenus { get; set; } = new List<RiMenu>();
        public List<RiModelo> RiModelos { get; set; } = new List<RiModelo>();
        public List<RecGrupo> RecGrupos { get; set; } = new List<RecGrupo>();
    }
}