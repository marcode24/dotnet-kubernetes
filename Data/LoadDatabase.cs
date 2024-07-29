using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase {
  public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager) {
    if(!usuarioManager.Users.Any()) {
      var usuario = new Usuario {
        Nombre = "Test",
        Apellido = "Test",
        Email = "test@gmail.com",
        UserName = "Test123",
        PhoneNumber = "1234567890"
      };

      await usuarioManager.CreateAsync(usuario, "MySecretTest123$");
    }

    if(!context.Inmuebles.Any()) {
      context.Inmuebles.AddRange(
        new Inmueble {
          Nombre = "Casa",
          Direccion = "Calle 123",
          Precio = 100M,
          FechaCreacion = DateTime.Now
        },
        new Inmueble {
          Nombre = "Apartamento",
          Direccion = "Calle 456",
          Precio = 200M,
          FechaCreacion = DateTime.Now
        },
        new Inmueble {
          Nombre = "Oficina",
          Direccion = "Calle 789",
          Precio = 300M,
          FechaCreacion = DateTime.Now
        }
      );

      await context.SaveChangesAsync();
    }
  }
}