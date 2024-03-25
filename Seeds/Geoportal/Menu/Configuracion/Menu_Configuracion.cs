using server.Models;

namespace server.Seeds
{
  public static class Seeds_Geoportal_Menu_Configuracion
  {
    public static RiMenu Usuarios = new()
    {
      Nombre = "Usuarios",
      Accion = "/dashboard/usuarios",
      Padre = Seeds_Geoportal_Menu.ConfiguracionPadre,
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static RiMenu Grupos = new()
    {
      Nombre = "Grupos",
      Accion = "/dashboard/grupos",
      Padre = Seeds_Geoportal_Menu.ConfiguracionPadre,
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static List<RiMenu> List = new List<RiMenu>()
    {
      Usuarios,
      Grupos
    };
  }
}