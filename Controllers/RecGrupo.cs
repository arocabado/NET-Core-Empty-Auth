using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Utils;
using server.Dtos;
using server.Constants;
using Microsoft.AspNetCore.Authorization;
using server.Extensions;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RecGrupoController : ControllerBase
    {
        private readonly DBContext db;
        private readonly Response response;

        public RecGrupoController(DBContext _db)
        {
            db = _db;
            response = new Response();
        }

        // GET: /recGrupo
        [HttpGet]
        public async Task<IResult> Get()
        {

            var grupo = await db.RecGrupo.GetRes().ToListAsync();

            return response.SuccessResponse(Messages.RecGrupo.GET, grupo);
        }

        // GET: /recGrupo/all
        [HttpGet("all")]
        public async Task<IResult> GetAll()
        {
            var grupo = await db.RecGrupo.IgnoreQueryFilters().Includes().ToListAsync();
            return response.SuccessResponse(Messages.RecGrupo.GET, grupo);
        }

        // GET: /recGrupo/{id}
        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            var grupo = await db.RecGrupo.Include(x => x).Where(v => v.Id == Guid.Parse(id)).FirstOrDefaultAsync();
            if (grupo == null)
            {
                return response.NotFoundResponse(Messages.RecGrupo.NOTFOUND);
            }
            return response.SuccessResponse(Messages.RecGrupo.FIND, grupo);
        }

        // POST: /recGrupo
        [HttpPost]
        public async Task<IResult> Post(RecGrupoDTO grupoDto)
        {
            var exists = await db.RecGrupo.AnyAsync(e => e.Nombre == grupoDto.Nombre);
            if (exists)
            {
                return response.BadRequestResponse(Messages.RecGrupo.EXISTS);
            }

            var id = db.GetProjectId();
            var project = await db.Proyectos.FirstOrDefaultAsync(item => item.Id == id);

            if (project == null) return response.NotFoundResponse(Messages.Auth.NOT_FOUND_PROJECT);


            var grupo = new RecGrupo
            {
                Nombre = grupoDto.Nombre,
                Descripcion = grupoDto.Descripcion,
                IdProyecto = project.Id,
                Proyecto = project
            };

            db.RecGrupo.Add(grupo);
            await db.SaveChangesAsync();

            return response.SuccessResponse(Messages.RecGrupo.CREATED, RecGrupo_Extensions.CreateRes(grupo));
        }

        // PUT: /recGrupo/{id}
        [HttpPut("{id}")]
        public async Task<IResult> Put(string id, RecGrupoDTO grupoDto)
        {
            Guid idGuid = Guid.Parse(id);
            var exists = await db.RecGrupo.AnyAsync(e => e.Nombre == grupoDto.Nombre && e.Id != idGuid);
            if (exists)
            {
                return response.BadRequestResponse(Messages.RecGrupo.EXISTS);
            }

            var grupo = await db.RecGrupo.Includes().FirstOrDefaultAsync(r => r.Id == idGuid);
            if (grupo == null)
            {
                return response.NotFoundResponse(Messages.RecGrupo.NOTFOUND);
            }

            grupo.Nombre = grupoDto.Nombre;
            grupo.Descripcion = grupoDto.Descripcion;

            await db.SaveChangesAsync();

            return response.SuccessResponse(Messages.RecGrupo.UPDATED, grupo);
        }

        // DELETE: /recGrupo/{id}
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(string id)
        {
            Guid idGuid = Guid.Parse(id);
            var grupo = await db.RecGrupo.Include(r => r.Menus).FirstOrDefaultAsync(r => r.Id == idGuid);
            if (grupo == null)
            {
                return response.NotFoundResponse(Messages.RecGrupo.NOTFOUND);
            }

            db.RecGrupo.Remove(grupo);
            await db.SaveChangesAsync();

            return response.SuccessResponse(Messages.RecGrupo.DELETED, "");
        }


        [HttpPut("accesos/{id}")]
        public async Task<IResult> UpdateAccess(string id, List<MenuAccesoDTO> body)
        {

            var grupo = await db.RecGrupo
                      .Includes()
                      .Include(rg => rg.Usuarios)
                      .FirstOrDefaultAsync(r => r.Id == Guid.Parse(id));

            if (grupo is null) return response.NotFoundResponse(Messages.RecGrupo.NOTFOUND);

            var idsMenu = body.Select(r => Guid.Parse(r.IdMenu)).ToList();

            grupo.Menus = grupo.Menus
                .Where(r => idsMenu.Contains(r.Id))
                .ToList();

            var menuActuales = grupo.Menus
                .Select(r => r.Id)
                .ToList();

            var nuevosMenus = idsMenu.Except(menuActuales).ToList();

            foreach (Guid idMenu in nuevosMenus)
            {
                var menu = await db.RiMenu.FindAsync(idMenu);
                if (menu != null)
                {
                    grupo.Menus.Add(menu);
                }
            }

            var accesos = body.SelectMany(r => r.Accesos).ToList();
            grupo.Accesos = grupo.Accesos
                .Where(r => accesos.Select(a => a.IdModelo).Contains(r.IdModelo))
                .ToList();

            var accesosActuales = grupo.Accesos
                .Select(r => r.IdModelo)
                .ToList();
            var existentes = accesos.Where(a => accesosActuales.Contains(a.IdModelo)).ToList();
            var nuevoAccesos = accesos.Where(a => !accesosActuales.Contains(a.IdModelo)).ToList();

            foreach (var acceso in existentes)
            {
                var a = grupo.Accesos.Where(a => a.IdModelo == acceso.IdModelo).FirstOrDefault();
                if (a != null)
                {
                    a.Crear = acceso.Agregar;
                    a.Editar = acceso.Editar;
                    a.Ver = acceso.Ver;
                    a.Eliminar = acceso.Eliminar;
                    a.FechaModificacion = DateTime.UtcNow;
                }
            }

            foreach (var acceso in nuevoAccesos)
            {
                var modelo = await db.RiModelo.FindAsync(acceso.IdModelo);
                if (modelo != null)
                {
                    var accesoModelo = new RiAccesoModelo
                    {
                        Grupo = grupo,
                        Ver = acceso.Ver,
                        Crear = acceso.Agregar,
                        Editar = acceso.Editar,
                        Eliminar = acceso.Eliminar,
                        FechaCreacion = DateTime.UtcNow,
                        FechaModificacion = DateTime.UtcNow,
                        Modelo = modelo
                    };
                    grupo.Accesos.Add(accesoModelo);
                }
            }

            await db.SaveChangesAsync();

            foreach (var user in grupo.Usuarios)
            {
                await WSManager.BroadcastOne(user.Id, new IResponseSocket
                {
                    Data = "",
                    Message = "Tus accesos han cambiado",
                    Type = Sockets.Types.USERGROUP
                });
            }

            return response.SuccessResponse(Messages.RecGrupo.UPDATED, RecGrupo_Extensions.CreateRes(grupo));
        }
    }
}