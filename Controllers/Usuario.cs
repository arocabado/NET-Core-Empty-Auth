using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Data;
using server.Utils;
using server.Extensions;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Responses;
using server.Dtos;

[Route("[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly DBContext db;
    private readonly IConfiguration _config;
    private readonly Response res;

    public UsuariosController(DBContext _db, IConfiguration config)
    {
        db = _db;
        _config = config;
        res = new Response();
    }

    [HttpGet]
    public async Task<IResult> GetUsuarios()
    {

        var id = db.GetTokenId();
        var user = await db.Usuario.FirstOrDefaultAsync(item => item.Id == id);

        var Usuario = await db.Usuario.Where(v => v.IdProyecto == user!.IdProyecto).GetRes().ToListAsync();
        return res.SuccessResponse(Messages.Usuario.GET, Usuario);
    }


    [HttpGet("{id}")]
    public async Task<IResult> GetUsuario(Guid id)
    {
        return await db.Usuario.Where(ca => ca.Id == id).GetRes().FirstOrDefaultAsync() is Usuario_Response e
             ? res.SuccessResponse(Messages.Usuario.FIND, e)
             : res.NotFoundResponse(Messages.Usuario.NOTFOUND);
    }

    [HttpPost]
    public async Task<IResult> PostUsuario(UsuarioDTO ca)
    {
        var projectId = db.GetProjectId();

        var exit = await db.Usuario.AnyAsync(e => e.Login == ca.Login);
        if (exit) return res.BadRequestResponse(Messages.Usuario.EXISTS);

        var project = await db.Proyectos.FirstOrDefaultAsync(e => e.Id == projectId);
        if (project is null) return res.NotFoundResponse(Messages.Auth.NOT_FOUND_PROJECT);

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(ca.Password);

        Usuario usuario = new()
        {
            Telefono = ca.Telefono,
            Login = ca.Login,
            Password = hashedPassword,
            CodigoSecreto ="" ,
            Firma = "",
            Notificacion = "",
            Verificado =null,
            IdProyecto = project.Id,
            Proyecto = project,
        };

        db.Usuario.Add(usuario);
        await db.SaveChangesAsync();
        return res.SuccessResponse(Messages.Usuario.CREATED, Usuario_Extensions.CreateRes(usuario));
    }

    [HttpPut("grupos/{id}")]
    public async Task<IResult> GruposDeUsuario(Guid id, UsuarioGruposDTO dto)
    {
        var usuario = await db.Usuario.Includes().FirstOrDefaultAsync(c => c.Id == id);
        if (usuario is null) return res.NotFoundResponse(Messages.Usuario.NOTFOUND);

        usuario.Grupos = usuario.Grupos
          .Where(r => dto.Idsgrupo.Contains(r.Id))
          .ToList();

        var grupoActuales = usuario.Grupos
            .Select(r => r.Id)
            .ToList();

        var nuevoGrupos = dto.Idsgrupo.Except(grupoActuales).ToList();

        foreach (Guid idGrupo in nuevoGrupos)
        {
            var grupo = await db.RecGrupo.FindAsync(idGrupo);
            if (grupo != null)
            {
                usuario.Grupos.Add(grupo);
            }
        }

        await db.SaveChangesAsync();

        /* await WSManager.BroadcastOne(usuario.Id, new IResponseSocket
        {
            Data = "",
            Message = "Tus accesos han cambiado",
            Type = Sockets.Types.USERGROUP
        }); */

        return res.SuccessResponse(Messages.Usuario.UPDATED, Usuario_Extensions.CreateRes(usuario));
    }

    [HttpPut("{id}")]
    public async Task<IResult> PutUsuario(Guid id, UsuarioDTO req)
    {
        var exit = await db.Usuario.AnyAsync(e => e.Login == req.Login && e.Id != id);
        if (exit) return res.BadRequestResponse(Messages.Usuario.EXISTS);

        var usuario = await db.Usuario.Includes().FirstOrDefaultAsync(c => c.Id == id);
        if (usuario is null) return res.NotFoundResponse(Messages.Usuario.NOTFOUND);

        usuario.Telefono = req.Telefono;
        usuario.Login = req.Login;
        /* usuario.Password = BCrypt.Net.BCrypt.HashPassword(req.Password); */
        usuario.CodigoSecreto = "";
        usuario.Firma = "";
        usuario.Notificacion = "";
        usuario.Verificado = true;
        await db.SaveChangesAsync();
        return res.SuccessResponse(Messages.Usuario.UPDATED, Usuario_Extensions.CreateRes(usuario));
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IResult> DeleteUsuario(Guid id)
    {
        var Usuario = await db.Usuario.Include(c => c.Proyecto).FirstOrDefaultAsync(c => c.Id == id);
        if (Usuario is null) return res.NotFoundResponse(Messages.Usuario.NOTFOUND);
        db.Usuario.Remove(Usuario);
        await db.SaveChangesAsync();
        return res.SuccessResponse(Messages.Usuario.DELETED, "");
    }
}