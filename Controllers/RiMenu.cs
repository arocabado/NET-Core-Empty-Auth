using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Data;
using server.Dtos;
using server.Extensions;
using server.Helpers;
using server.Models;
using server.Responses;
using server.Utils;

namespace server.Controllers
{
    [Authorize]
    [Route("riMenu")]
    [ApiController]
    public class RiMenuController : ControllerBase
    {
        private readonly DBContext db;
        private readonly Response res;

        public RiMenuController(DBContext _db)
        {
            db = _db;
            res = new Response();
        }

         [HttpGet("dashboard")]
        public async Task<IResult> GetDashboard()
        {
            var tokenId = User.FindFirst("Id")?.Value;
            if (tokenId == null) return res.BadRequestResponse(Messages.Auth.ERROR_TOKEN);

            Guid id = Guid.Parse(tokenId);
            var user = await db.Usuario
                           .Include(ru => ru.Grupos)
                               .ThenInclude(g => g.Menus)
                           .Include(ru => ru.Grupos)
                               .ThenInclude(rug => rug.Accesos)
                           .FirstOrDefaultAsync(ru => ru.Id == id);

            if (user == null) return res.BadRequestResponse(Messages.Usuario.NOTFOUND);

            var menuIds = user.Grupos
                .Select(rug => rug.Menus.Select(rmgr => rmgr.Id))
                .SelectMany(ids => ids)
                .ToList();

            var menus = await db.RiMenu
                .OrderBy(m => m.Secuencia)
                .Where(m => m.IdPadre == null && menuIds.Contains(m.Id))
                .ToListAsync();

            var links = new List<ParentLink>();
            foreach (var m in menus)
            {
                var childs = await RiMenu_Helpers.RecursiveGetLinks(db, menuIds, m);
                var link = new ParentLink
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Icon = m.PathIcono,
                    Hijos = childs
                };
                links.Add(link);
            }

            var accesoIds = user.Grupos
                .SelectMany(rug => rug.Accesos.Select(ram => ram.Id))
                .ToList();

            var accesos = await db.RiAccesoModelo
                .Where(ram => accesoIds.Contains(ram.Id))
                .GetRes()
                .ToListAsync();

            return res.SuccessResponse("Dashboard obtenido correctamente", new
            {
                Menus = links,
                Accesos = accesos
            });
        } 

        [HttpGet]
        public async Task<IResult> GetMenus()
        {
            var menus = await RiMenu_Helpers.RecursiveGetMenus(db, null);
            return res.SuccessResponse(Messages.RiMenu.GET, menus);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMenus()
        {
            var Menu = await db.RiMenu.IgnoreQueryFilters().GetRes().ToListAsync();
            return Ok(res.SuccessResponse(Messages.RiMenu.GET, Menu));
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetMenu(string id)
        {
            var Menu = await db.RiMenu.Where(ca => ca.Id == Guid.Parse(id)).GetRes().FirstOrDefaultAsync() is RiMenuRes e
                   ? res.SuccessResponse(Messages.RiMenu.FIND, e)
                   : res.NotFoundResponse(Messages.RiMenu.NOTFOUND);
            return Menu;
        }

        [HttpPost]
        public async Task<IResult> CreateMenu(RiMenuDTO ca)
        {
            var projectId = db.GetProjectId();
            var project = await db.Proyectos.FirstOrDefaultAsync(item => item.Id == projectId);

            var fecha = DateTime.UtcNow;
            var exit = await db.RiMenu.AnyAsync(e => e.Nombre == ca.Nombre);
            if (exit) return res.BadRequestResponse(Messages.RiMenu.EXISTS);
            RiMenu? padre = null;

            if (ca.IdPadre != null)
            {
                Guid? idPadre = Guid.Parse(ca.IdPadre);
                padre = await db.RiMenu.FirstOrDefaultAsync(rm => rm.Id == idPadre);
                if (padre == null) return res.NotFoundResponse(Messages.RiMenu.NOTPADRE);
            }

            if (project == null) return res.NotFoundResponse(Messages.Auth.NOT_FOUND_PROJECT);

            RiMenu menu = new()
            {
                Nombre = ca.Nombre,
                Accion = ca.Accion,
                PathIcono = ca.PathIcono,
                PathPadre = ca.PathPadre,
                Secuencia = ca.Secuencia,
                Padre = padre,
                Proyecto = project,
                ProyectoId = project.Id
            };
            db.RiMenu.Add(menu);
            await db.SaveChangesAsync();
            return res.SuccessResponse(Messages.RiMenu.CREATED, RiMenu_Extensions.CreateRes(menu));
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateMenu(string id, RiMenuDTO ca)
        {
            var menu = await db.RiMenu.Includes().FirstOrDefaultAsync(rm => rm.Id == Guid.Parse(id));
            if (menu is null) return res.NotFoundResponse(Messages.RiMenu.NOTFOUND);
            RiMenu? padre = null;

            if (ca.IdPadre != null)
            {
                Guid? IdPadre = Guid.Parse(ca.IdPadre);
                padre = await db.RiMenu.FirstOrDefaultAsync(rm => rm.Id == IdPadre);
                if (padre == null) return res.NotFoundResponse(Messages.RiMenu.NOTPADRE);
            }
            menu.Nombre = ca.Nombre;
            menu.PathPadre = ca.PathPadre;
            menu.PathIcono = ca.PathIcono;
            menu.Accion = ca.Accion;
            menu.Secuencia = ca.Secuencia;
            menu.Padre = padre;

            await db.SaveChangesAsync();
            return res.SuccessResponse(Messages.RiMenu.UPDATED, RiMenu_Extensions.CreateRes(menu));
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteMenu(string id)
        {
            var menu = await db.RiMenu.Include(m => m.Grupos).FirstOrDefaultAsync(m => m.Id == Guid.Parse(id));
            if (menu is null) return res.NotFoundResponse(Messages.RiMenu.NOTFOUND);
            db.RiMenu.Remove(menu);
            await db.SaveChangesAsync();
            return res.SuccessResponse(Messages.RiMenu.DELETED, "");
        }



    }



}