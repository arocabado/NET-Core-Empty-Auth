using server.Models;

namespace server.Responses
{
  public class User_Logged_Response
  {
    public required Guid Id { get; set; }
    public required string? Login { get; set; }
    public required string? Telefono { get; set; }
  }

  public class Usuario_Response : Base_Response
  {
    public required Guid Id { get; set; }
    public required string? Login { get; set; }
    public required string Password { get; set; }
    public required string? Telefono { get; set; }
    public required bool? Verificado { get; set; }
    public required string? CodigoSecreto { get; set; }
    public required string? Firma { get; set; }
    public required string? Notificacion { get; set; }

    public required List<RecGrupoRes> Grupos { get; set; }
  }
}