using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Responses;


namespace server.Extensions
{
    public static class RecGrupo_Extensions
    {
        public static RecGrupoRes CreateRes(RecGrupo rg)
        {
            return new RecGrupoRes
            {
                Id = rg.Id,
                IdProyecto = rg.IdProyecto,
                FechaCreacion = rg.FechaCreacion,
                FechaModificacion = rg.FechaModificacion,
                IdUsrCreacion = rg.IdUsrCreacion,
                IdUsrModificacion = rg.IdUsrModificacion,
                Nombre = rg.Nombre,
                Descripcion = rg.Descripcion,
                Estado = rg.Estado,
                Menus = rg.Menus.AsQueryable().GetRes().ToList(),
                Accesos = rg.Accesos.AsQueryable().GetRes().ToList()
            };
        }

        public static IQueryable<RecGrupo> Includes(this IQueryable<RecGrupo> query)
        {
            return query
                .Include(rg => rg.Accesos)
                .ThenInclude(rg => rg.Modelo)
                .Include(rg => rg.Menus);
        }

        public static IQueryable<RecGrupoRes> GetRes(this IQueryable<RecGrupo> query)
        {
            return query.Includes().Select(entity => CreateRes(entity));
        }

    }
}