using server.Constants;
using server.Data;
using server.Extensions;
using server.Models;

namespace server.Seeds
{
  public class DataSeeder_Geoportal
  {
    private readonly DBContext dbContext;
    public DataSeeder_Geoportal(DBContext _dbContext)
    {
      dbContext = _dbContext;
    }

    public void Seed()
    {
      // USUARIOS
      dbContext.Usuario.AddRange(Seeds_Geoportal_Usuario.List);

      // MENUS
      var menuList = new List<RiMenu>();
      menuList.AddRange(Seeds_Geoportal_Menu.List.OrderSequence());
      menuList.AddRange(Seeds_Geoportal_Menu_Mapas.List.OrderSequence());
      menuList.AddRange(Seeds_Geoportal_Menu_Configuracion.List.OrderSequence());

      // GRUPOS
      // ASIGNAR PERMISOS INICIALES AL GRUPO TODOS
      Seeds_Geoportal_Grupo.Todo.Usuarios.Add(Seeds_Geoportal_Usuario.Admin);
      Seeds_Geoportal_Grupo.Todo.Menus.AddRange(menuList);
      dbContext.RecGrupo.AddRange(Seeds_Geoportal_Grupo.List);

      dbContext.RiMenu.AddRange(menuList);
    }
  }
}

