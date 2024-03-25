namespace server.Responses
{
  public class Base_Response
  {
    public required Guid? IdUsrCreacion { get; set; }
    public required Guid? IdUsrModificacion { get; set; }
    public required string Estado { get; set; }
    public required DateTime? FechaModificacion { get; set; }
    public required DateTime? FechaCreacion { get; set; }
  }
}
