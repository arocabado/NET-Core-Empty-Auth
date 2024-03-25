namespace server.Models
{
        public class RiMenu : Base
        {
                public Guid? IdPadre { get; set; }
                public Guid ProyectoId { get; set; }
                public required string Nombre { get; set; }
                public int Secuencia { get; set; }
                public string? PathIcono { get; set; } = null;
                public string PathPadre { get; set; } = "";
                public string? Accion { get; set; } = null;
                public RiMenu? Padre { get; set; } = null;
                public List<RiModelo> Modelos { get; set; } = new List<RiModelo>();
                public List<RecGrupo> Grupos { get; set; } = new List<RecGrupo>();
                public required Proyecto Proyecto { get; set; }
        }
}
