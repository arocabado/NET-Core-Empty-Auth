namespace server.Models
{
    public class RiModelo : Base
    {
        public Guid IdMenu { get; set; }
        public Guid IdProyecto { get; set; }
        public required string NombreModelo { get; set; }
        public required string Descripcion { get; set; }
        public required string Tipo { get; set; }
        public required RiMenu Menu { get; set; }
        public List<RiAccesoModelo> Accesos = new List<RiAccesoModelo>();
    }
}
