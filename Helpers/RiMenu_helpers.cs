using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Extensions;
using server.Models;
using server.Responses;

namespace server.Helpers
{
    public static class RiMenu_Helpers
    {
        public static async Task<List<RiMenuRes>> RecursiveGetMenus(DBContext db, Guid? padre)
        {

            var menus = await db.RiMenu.OrderBy(m => m.Secuencia).Where(m => m.IdPadre == padre).GetRes().ToListAsync();
            var list = new List<RiMenuRes>();
            if (menus.Count() == 0)
            {
                return list;
            }
            foreach (var menu in menus)
            {
                var childs = await RecursiveGetMenus(db, menu.Id);
                list.Add(menu);
                list.AddRange(childs);
            }
            return list;
        }

        public static async Task<List<ChildLink>> RecursiveGetLinks(DBContext db, List<Guid> menuIds, RiMenu menu)
        {
            var menus = await db.RiMenu
                .Where(m => m.IdPadre == menu.Id && menuIds.Contains(m.Id))
                .Include(m => m.Padre)
                .OrderBy(m => m.Secuencia)
                .ToListAsync();
            var links = new List<ChildLink>();
            if (menus.Count == 0)
            {
                return links;
            }
            foreach (var m in menus)
            {
                var childs = await RecursiveGetLinks(db, menuIds, m);
                var link = new ChildLink
                {
                    Id = m.Id,
                    Padre = m.Padre?.Nombre ?? "",
                    Nombre = m.Nombre,
                    Accion = m.Accion,
                    Hijos = childs
                };
                links.Add(link);
            }
            return links;
        }
        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out int i)) return i;
            return null;
        }
    }
}