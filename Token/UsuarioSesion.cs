using System.Security.Claims;

namespace NetKubernetes.Token;

public class UsusarioSesion : IUsuarioSesion
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UsusarioSesion(IHttpContextAccessor httpContextAccessor) {
    _httpContextAccessor = httpContextAccessor;
  }

  public string ObtenerUsuarioSesion() {
    var userName = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

    return userName ?? string.Empty;
  }
}