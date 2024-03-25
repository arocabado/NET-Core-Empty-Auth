namespace server.Dtos
{
    public class RiAccesoModeloDTO
    {
        public required int IdGrupo { get; set; }
        public required int IdModelo { get; set; }
        public required bool Ver { get; set; }
        public required bool Crear { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DateTime FechaModificacion { get; set; }
    }
    public class AccesoDTO
    { 
        public required bool Ver { get; set; }
        public required bool Agregar { get; set; }
        public required bool Crear { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }
        public required Guid IdModelo { get;set; }
    }

}
