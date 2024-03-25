using server.Dtos;
using server.Models;

namespace server.Responses
{
    public class RiAccesoModeloRes : Base_Response
    {
          public required Guid Id { get; set; }
        public required Guid IdGrupo { get; set; }
        public required Guid IdModelo { get; set; }
        public required string NombreModelo { get; set; }
        public required bool Ver { get; set; }
        public required bool Crear { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }
    }

    public class MenuAccesoRes:Base_Response
    {
        public required Guid IdMenu { get; set; }
        public required List<AccesoaRes> Accesos { get; set; }
    }
    
    public class AccesoaRes 
    {
        public required Guid IdModelo { get; set; }
        public required bool Ver { get; set; }
        public required bool Crear { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }
        public required string Estado { get; set; }

    }
}
