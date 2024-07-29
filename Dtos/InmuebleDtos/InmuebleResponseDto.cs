namespace NetKubernetes.Dtos.InmuebleDtos;

public class InmuebleResponseDto {
  public int Id { get; set; }
  public string Nombre { get; set; } = string.Empty;
  public string Direccion { get; set; } = string.Empty;
  public decimal Precio { get; set; }
  public string Picture { get; set; } = string.Empty;
  public DateTime FechaCreacion { get; set; }
}