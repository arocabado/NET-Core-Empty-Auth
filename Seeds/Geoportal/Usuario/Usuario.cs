using server.Models;

namespace server.Seeds
{
  public static class Seeds_Geoportal_Usuario
  {
    public static Usuario Admin = new()
    {
      Login = "admin",
      Password = "$2b$10$HLRudu6YULNxULnFE4o3AeohXd3/dDclz3eVYP8kuvDIHQg4/.N02", //123456
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static List<Usuario> List = new()
    {
      Admin
    };
  }
}