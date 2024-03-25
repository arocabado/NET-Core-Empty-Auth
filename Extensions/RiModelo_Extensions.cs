using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Responses;

namespace server.Extensions
{
    public static class RiModelo_Extensions
    {
        public static RiModeloRes CreateRes(RiModelo rm)
        {
            return new RiModeloRes
            {
                Id = rm.Id,
                Modelo = rm.NombreModelo,
                Estado = rm.Estado,
                Descripcion = rm.Descripcion,
                Tipo = rm.Tipo,
                IdMenu = rm.IdMenu,
                NombreMenu = rm.Menu.Nombre,
                IdUsrCreacion = rm.IdUsrModificacion,
                IdUsrModificacion = rm.IdUsrModificacion,
                FechaCreacion = rm.FechaCreacion,
                FechaModificacion = rm.FechaModificacion,
            };
        }
        public static IQueryable<RiModelo> Includes(this IQueryable<RiModelo> query)
        {
            return query.Include(rm => rm.Menu);
        }
        public static IQueryable<RiModeloRes> GetRes(this IQueryable<RiModelo> query)
        {
            return query.Includes().Select(entity => CreateRes(entity));
        }

    }
}