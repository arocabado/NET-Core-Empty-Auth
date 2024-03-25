using auth.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Data;
using server.Dtos;
using server.Extensions;
using server.Models;
using server.Utils;

namespace server.Controllers
{
  [ApiController]
  [Route("auth/v1")]
  public class AuthV1Controller : ControllerBase
  {
    private readonly DBContext _db;
    private readonly IConfiguration _config;
    private readonly Response res;

    public AuthV1Controller(DBContext db, IConfiguration config)
    {
      _db = db;
      _config = config;
      res = new Response();
    }

    [HttpPost("register")]
    public async Task<IResult> Register(RegisterDTO body)
    {
      var existUser = await _db.Usuario
        .Include(v => v.Proyecto)
        .AnyAsync(v => v.Login == body.Login && v.Proyecto.Nombre == body.ProjectCluster);
      if (existUser) return res.BadRequestResponse(Messages.Auth.EXISTS_USERNAME);

      var project = await _db.Proyectos.FirstOrDefaultAsync(v => v.Nombre == body.ProjectCluster);
      if (project == null) return res.BadRequestResponse(Messages.Auth.NOT_FOUND_PROJECT);

      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(body.Password);
      var newUser = new Usuario
      {
        Proyecto = project,
        Login = body.Login,
        Password = hashedPassword
      };

      _db.Usuario.Add(newUser);
      await _db.SaveChangesAsync();
      return res.SuccessResponse(Messages.Auth.CREATED, Usuario_Extensions.CreateLoggedRes(newUser));
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginDTO body)
    {
      var user = await _db.Usuario
        .Include(u => u.Proyecto)
        .Where(u => u.Login == body.Login && u.Proyecto.Nombre == body.ProjectCluster)
        .FirstOrDefaultAsync();
      if (user == null) return res.NotFoundResponse(Messages.Auth.NOT_FOUND);
      bool hash = BCrypt.Net.BCrypt.Verify(body.Password, user.Password);
      if (user == null || !hash) return res.NotFoundResponse(Messages.Auth.UNAUTHORIZED);
      var key = _config.GetSection("JWT:Key").Value;
      var issuer = _config.GetSection("JWT:Issuer").Value;
      var audience = _config.GetSection("JWT:Audience").Value;
      if (key == null || issuer == null || audience == null) return res.BadRequestResponse(Messages.Auth.ERROR_CONFIG);

      var token = AuthUtility.GenerateToken(user, new JwtSecrets
      {
        Key = key,
        Audience = audience,
        Issuer = issuer
      });

      return res.SuccessResponse(Messages.Auth.FOUND, token);
    }


    [HttpGet("me"), Authorize]
    public async Task<IResult> GetUserByToken()
    {
      var id = _db.GetTokenId();
      if (id == null) return res.BadRequestResponse(Messages.Auth.ERROR_TOKEN);
      var user = await _db.Usuario.FirstOrDefaultAsync(item => item.Id == id);
      if (user == null) return res.NotFoundResponse(Messages.Auth.NOT_FOUND);
      return res.SuccessResponse(Messages.Auth.FOUND, Usuario_Extensions.CreateLoggedRes(user));
    }

    [HttpPatch("password"), Authorize]
    public async Task<IResult> ChangePassword(CambiarContraDTO body)
    {
      var id = _db.GetTokenId();
      if (id == null) return res.BadRequestResponse(Messages.Auth.ERROR_TOKEN);

      var user = await _db.Usuario.FindAsync(id);
      if (user == null) return res.NotFoundResponse(Messages.Auth.NOT_FOUND);

      if (body.Actual.Trim() == "" || body.Confirm.Trim() == "" || body.New.Trim() == "") return res.BadRequestResponse(Messages.Auth.REQUIRED);

      bool hash = BCrypt.Net.BCrypt.Verify(body.Actual, user.Password);
      if (!hash) return res.BadRequestResponse(Messages.Auth.ERROR_PASSWORD_ACTUAL);

      if (body.New != body.Confirm) return res.BadRequestResponse(Messages.Auth.ERROR_PASSWORD_BODY);

      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(body.New);
      user.Password = hashedPassword;
      await _db.SaveChangesAsync();
      return res.SuccessResponse(Messages.Auth.UPDATED_PASSWORD, Usuario_Extensions.CreateLoggedRes(user));
    }
  }
}