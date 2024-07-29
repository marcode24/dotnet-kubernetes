namespace NetKubernetes.Dtos.UsuarioDtos;

public class UsuarioRegistroRequestDto {
  public string Nombre { get; set; } = string.Empty;
  public string Apellido { get; set; } = string.Empty;
  public string Telefono { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}