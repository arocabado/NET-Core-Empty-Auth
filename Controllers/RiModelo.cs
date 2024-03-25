using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using server.Data;
using server.Models;
using server.Utils;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Responses;
using server.Extensions;
using server.Dtos;


namespace server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]


    public class RiModeloController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly Response _res;

        public RiModeloController(DBContext db)
        {
            _db = db;
            _res = new Response();
        }



        [HttpGet]
        public async Task<IResult> Get()
        {
            var modelo = await _db.RiModelo.GetRes().ToListAsync();
            return _res.SuccessResponse(Messages.RiModelo.GET, modelo);
        }

        [HttpGet("all")]
        public async Task<IResult> GetAll()
        {
            var modelo = await _db.RiModelo.GetRes().ToListAsync();
            return _res.SuccessResponse(Messages.RiModelo.GET, modelo);
        }

        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            var modelo = await _db.RiModelo.Where(ca => ca.Id == Guid.Parse(id)).GetRes().FirstOrDefaultAsync();
            if (modelo == null)
            {
                return _res.NotFoundResponse(Messages.RiModelo.NOTFOUND);
            }
            return _res.SuccessResponse(Messages.RiModelo.FIND, modelo);
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] RiModeloDTO ca)
        {
            var exists = await _db.RiModelo.AnyAsync(e => e.NombreModelo == ca.Modelo);
            if (exists)
            {
                return _res.BadRequestResponse(Messages.RiModelo.EXISTS);
            }
            var cc = await _db.RiMenu.FindAsync(Guid.Parse(ca.IdMenu));
            if (cc == null)
            {
                return _res.NotFoundResponse(Messages.RiMenu.NOTFOUND);
            }

            var id = _db.GetTokenId();
            var user = await _db.Usuario.FirstOrDefaultAsync(item => item.Id == id);

            RiModelo modelo = new()
            {
                NombreModelo = ca.Modelo,
                Descripcion = ca.Descripcion,
                Tipo = ca.Tipo,
                Menu = cc,
                IdProyecto = user!.IdProyecto
            };
            _db.RiModelo.Add(modelo);
            await _db.SaveChangesAsync();
            return _res.SuccessResponse(Messages.RiModelo.CREATED, RiModelo_Extensions.CreateRes(modelo));
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(string id, [FromBody] RiModeloDTO ca)
        {
            var exists = await _db.RiModelo.AnyAsync(e => e.NombreModelo == ca.Modelo && e.Id != Guid.Parse(id));
            if (exists)
            {
                return _res.BadRequestResponse(Messages.RiModelo.EXISTS);
            }
            var modelo = await _db.RiModelo.Includes().FirstOrDefaultAsync(rm => rm.Id == Guid.Parse(id));
            if (modelo == null)
            {
                return _res.NotFoundResponse(Messages.RiModelo.NOTFOUND);
            }
            modelo.NombreModelo = ca.Modelo;
            modelo.Tipo = ca.Tipo;
            modelo.Descripcion = ca.Descripcion;
            await _db.SaveChangesAsync();
            return _res.SuccessResponse(Messages.RiModelo.UPDATED, RiModelo_Extensions.CreateRes(modelo));
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(string id)
        {
            Guid idGuid = Guid.Parse(id);
            var modelo = await _db.RiModelo.Include(m => m.Accesos).FirstOrDefaultAsync(m => m.Id == idGuid);
            if (modelo == null)
            {
                return _res.NotFoundResponse(Messages.RiModelo.NOTFOUND);
            }
            _db.RiModelo.Remove(modelo);
            await _db.SaveChangesAsync();
            return _res.SuccessResponse(Messages.RiModelo.DELETED, "");
        }
    }
}