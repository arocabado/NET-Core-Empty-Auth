using server.Models;

namespace server.Seeds
{
  public static class Seeds_Geoportal_Grupo
  {
    public static RecGrupo Todo = new()
    {
      Nombre = "Todo",
      Descripcion = "Puede realizar todas las acciones",
      Proyecto = Seeds_Proyecto.Geoportal
    };

    public static List<RecGrupo> List = new()
    {
      Todo
    };
  }
}