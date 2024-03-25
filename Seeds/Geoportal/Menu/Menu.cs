using server.Extensions;
using server.Models;

namespace server.Seeds
{
  public static class Seeds_Geoportal_Menu
  {
    public static RiMenu MapasPadre = new()
    {
      Nombre = "Mapas",
      PathIcono = "Mapa",
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static RiMenu ConfiguracionPadre = new()
    {
      Nombre = "Configuracion",
      PathIcono = "Configuracion",
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static List<RiMenu> List = new List<RiMenu>()
    {
      MapasPadre,
      ConfiguracionPadre,
    };
  }
}