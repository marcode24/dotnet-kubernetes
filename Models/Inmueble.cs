using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetKubernetes.Models;

public class Inmueble {
  [Key]
  [Required]
  public int Id { get; set; }
  public string Nombre { get; set; } = string.Empty;
  public string Direccion { get; set; } = string.Empty;
  [Required]
  [Column(TypeName = "decimal(18, 4)")]
  public decimal Precio { get; set; } = 0;
  public string Picture { get; set; } = string.Empty;
  public DateTime FechaCreacion { get; set; }
  public Guid UsuarioId { get; set; } = Guid.Empty;
}