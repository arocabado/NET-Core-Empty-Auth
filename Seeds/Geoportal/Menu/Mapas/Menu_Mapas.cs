using server.Models;

namespace server.Seeds
{
  public static class Seeds_Geoportal_Menu_Mapas
  {
    public static RiMenu Planificacion = new()
    {
      Nombre = "Planificacion",
      Accion = "/dashboard/planificacion",
      Padre = Seeds_Geoportal_Menu.MapasPadre,
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static RiMenu Urbanismo = new()
    {
      Nombre = "Urbanismo",
      Accion = "/dashboard/urbanismo",
      Padre = Seeds_Geoportal_Menu.MapasPadre,
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static List<RiMenu> List = new List<RiMenu>()
    {
      Planificacion,
      Urbanismo
    };
  }
}