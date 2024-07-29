using System.Net;
using Microsoft.AspNetCore.Identity;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository {
  private readonly AppDbContext _context;
  private readonly IUsuarioSesion _usuarioSesion;
  private readonly UserManager<Usuario> _userManager;

  public InmuebleRepository(
    AppDbContext context,
    IUsuarioSesion usuarioSesion,
    UserManager<Usuario> userManager
  ) {
    _context = context;
    _usuarioSesion = usuarioSesion;
    _userManager = userManager;
  }
  public async Task CreateInmueble(Inmueble inmueble) {
    var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
    if(usuario is null) {
      throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El usuario del token no existe en la base de datos" });
    }

    if(inmueble is null) {
      throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El inmueble no puede ser nulo" });
    }

    inmueble.FechaCreacion = DateTime.Now;
    inmueble.UsuarioId = Guid.Parse(usuario!.Id);

    await _context.Inmuebles.AddAsync(inmueble);
  }

  public void DeleteInmueble(int id) {
    var inmueble = _context.Inmuebles.FirstOrDefault(x => x.Id == id);

    _context.Inmuebles.Remove(inmueble!);
  }

  public IEnumerable<Inmueble> GetAllInmuebles() {
    return [.. _context.Inmuebles];
  }

  public Inmueble GetInmuebleById(int id) {
    return _context.Inmuebles.FirstOrDefault(x => x.Id == id)!;
  }

  public bool SaveChanges() {
    return _context.SaveChanges() >= 0;
  }
}