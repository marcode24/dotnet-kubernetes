namespace NetKubernetes.Dtos.InmuebleDtos;

public class InmuebleRequestDto {
  public string Nombre { get; set; } = string.Empty;
  public string Direccion { get; set; } = string.Empty;
  public decimal Precio { get; set; }
  public string Picture { get; set; } = string.Empty;
}