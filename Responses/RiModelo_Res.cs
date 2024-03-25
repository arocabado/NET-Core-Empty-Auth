namespace server.Responses
{
    public class RiModeloRes : Base_Response
    {
        public required Guid Id { get; set; }
        public required Guid IdMenu { get; set; }
        public required string Modelo { get; set; }
        public required string Descripcion { get; set; }
        public required string Tipo { get; set; }

        public required string NombreMenu { get; set; }
    }
}
