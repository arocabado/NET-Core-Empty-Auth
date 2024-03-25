using server.Constants;
using server.Models;

namespace server.Seeds
{
  public static class Seeds_Proyecto
  {
    public static Proyecto Geoportal = new()
    {
      Nombre = "Geoportal",
      AuthVersion = AuthVersions.V1
    };
    public static List<Proyecto> List = new List<Proyecto>
    {
      Geoportal
    };
  }
}