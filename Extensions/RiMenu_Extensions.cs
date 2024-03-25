using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Responses;

namespace server.Extensions
{
    public static class RiMenu_Extensions
    {
        public static RiMenuRes CreateRes(RiMenu rm)
        {
            return new RiMenuRes
            {
                Id = rm.Id,
                FechaCreacion = rm.FechaCreacion,
                FechaModificacion = rm.FechaModificacion,
                Nombre = rm.Nombre,
                Accion = rm.Accion,
                IdPadre = rm.IdPadre,
                NombrePadre = rm.Padre?.Nombre,
                PathIcono = rm.PathIcono,
                PathPadre = rm.PathPadre,
                Secuencia = rm.Secuencia,
                Estado = rm.Estado,
                Modelos = rm.Modelos.AsQueryable().GetRes().ToList(),
                IdUsrCreacion = rm.IdUsrModificacion,
                IdUsrModificacion = rm.IdUsrModificacion,
            };
        }
        public static IQueryable<RiMenu> Includes(this IQueryable<RiMenu> query)
        {
            return query.Include(rm => rm.Padre).Include(rm => rm.Modelos);
        }
        public static IQueryable<RiMenuRes> GetRes(this IQueryable<RiMenu> query)
        {
            return query.Includes().Select(entity => CreateRes(entity));
        }
        public static List<RiMenu> OrderSequence(this List<RiMenu> list)
        {
            return list.Select((p, i) =>
            {
                p.Secuencia = i + 1;
                return p;
            }).ToList();
        }
    }
}