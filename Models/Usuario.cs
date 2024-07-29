using Microsoft.AspNetCore.Identity;

namespace NetKubernetes.Models;

public class Usuario : IdentityUser {
  public string Nombre { get; set; } = string.Empty;
  public string Apellido { get; set; } = string.Empty;
  public string Telefono { get; set; } = string.Empty;
}