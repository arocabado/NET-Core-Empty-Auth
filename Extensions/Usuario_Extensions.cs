using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Responses;

namespace server.Extensions
{
  public static class Usuario_Extensions
  {
    public static User_Logged_Response CreateLoggedRes(Usuario rm)
    {
      return new User_Logged_Response
      {
        Id = rm.Id,
        Login = rm.Login,
        Telefono = rm.Telefono,
      };
    }
    public static Usuario_Response CreateRes(Usuario rm)
    {
      return new Usuario_Response
      {
        Id = rm.Id,
        Login = rm.Login,
        Password = rm.Password,
        Telefono = rm.Telefono,
        Verificado = rm.Verificado,
        CodigoSecreto = rm.CodigoSecreto,
        Firma = rm.Firma,
        Notificacion = rm.Notificacion,
        Estado = rm.Estado,
        FechaCreacion = rm.FechaCreacion,
        FechaModificacion = rm.FechaModificacion,
        IdUsrCreacion = rm.IdUsrModificacion,
        IdUsrModificacion = rm.IdUsrModificacion,
        Grupos = rm.Grupos.AsQueryable().GetRes().ToList()
      };
    }
    public static IQueryable<Usuario> Includes(this IQueryable<Usuario> query)
    {
      return query.Include(v => v.Grupos);
    }
    public static IQueryable<Usuario_Response> GetRes(this IQueryable<Usuario> query)
    {
      return query.Includes().Select(entity => CreateRes(entity));
    }
  }
}