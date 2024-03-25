using server.Constants;

namespace server.Models
{
  public class Base
  {

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Estado { get; set; } = States.ACTIVE;
    public Guid? IdUsrCreacion { get; set; }
    public Guid? IdUsrModificacion { get; set; }
    public DateTime FechaModificacion { get; set; }
    public DateTime FechaCreacion { get; set; }

  }
}
