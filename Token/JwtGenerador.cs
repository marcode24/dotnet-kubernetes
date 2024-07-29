using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Models;

namespace NetKubernetes.Token;

public class JwtGenerador : IJwtGenerador
{
  public string CrearToken(Usuario usuario) {
    var claims = new List<Claim> {
      new(JwtRegisteredClaimNames.NameId, usuario.UserName ?? string.Empty),
      new(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),
    };

    // agregarlo a appsettings.json y obtenerlo desde allí
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var tokenDescripcion = new SecurityTokenDescriptor {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(7),
      SigningCredentials = credenciales,
    };

    var tokenManejador = new JwtSecurityTokenHandler();
    var token = tokenManejador.CreateToken(tokenDescripcion);

    return tokenManejador.WriteToken(token);
  }
}
