using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Models;
using auth.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace server.Utils
{
  public class AuthUtility : ControllerBase
  {

    public AuthUtility()
    {

    }
    public static string GenerateToken(Usuario user, JwtSecrets secrets)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.Key));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var claims = new List<Claim>(){
                new Claim("Id", user.Id.ToString()),
                new Claim("ProjectId", user!.IdProyecto.ToString())
            };

      var token = new JwtSecurityToken(
          secrets.Issuer,
          secrets.Audience,
          claims,
          expires: DateTime.Now.AddDays(1),
          signingCredentials: credentials);


      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //validate token
    private static TokenValidationParameters GetValidationParameters()
    {

      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

      IConfigurationRoot configuration = builder.Build();

      string validIssuer = configuration["JWT:Issuer"] ?? "";
      string ValidAudience = configuration["JWT:Audience"] ?? "";
      string IssuerSigningKey = configuration["JWT:Key"] ?? "";

      return new TokenValidationParameters()
      {
        ValidateLifetime = true, // Because there is no expiration in the generated token
        ValidateAudience = true, // Because there is no audiance in the generated token
        ValidateIssuer = true,   // Because there is no issuer in the generated token
        ValidIssuer = validIssuer,
        ValidAudience = ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey)) // The same key as the one that generate the token
      };
    }

    public static bool ValidateToken(string authToken)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var validationParameters = GetValidationParameters();

      SecurityToken validatedToken;
      try
      {
        tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }
  }
}