using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository {
  private readonly UserManager<Usuario> _userManager;
  private readonly SignInManager<Usuario> _signInManager;
  private readonly IJwtGenerador _jwtGenerador;
  private readonly AppDbContext _context;
  private readonly IUsuarioSesion _usuarioSesion;

  public UsuarioRepository(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager,
    IJwtGenerador jwtGenerador,
    AppDbContext context,
    IUsuarioSesion usuarioSesion
  ) {
    _userManager = userManager;
    _signInManager = signInManager;
    _jwtGenerador = jwtGenerador;
    _context = context;
    _usuarioSesion = usuarioSesion;
  }

  private UsuarioResponseDto TransformerUserToUserDto(Usuario usuario) {
    return new UsuarioResponseDto {
      Id = usuario.Id,
      Nombre = usuario.Nombre,
      Apellido = usuario.Apellido,
      Telefono = usuario.Telefono,
      Email = usuario.Email ?? string.Empty,
      UserName = usuario.UserName ?? string.Empty,
      Token = _jwtGenerador.CrearToken(usuario)
    };
  }

  public async Task<UsuarioResponseDto> GetUsuario() {
    var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
    if(usuario is null) {
      throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El usuario del token no existe en la base de datos"});
    }
    return TransformerUserToUserDto(usuario!);
  }

  public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLoginRequestDto) {
    var usuario = await _userManager.FindByEmailAsync(usuarioLoginRequestDto.Email);
    if(usuario is null) {
      throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El email del usuario no existe en la base de datos" });
    }

    var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, usuarioLoginRequestDto.Password!, false);
    if(resultado.Succeeded) {
      return TransformerUserToUserDto(usuario!);
    }

    throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Usuario y/o contraseña incorrectos" });
  }

  public async Task<UsuarioResponseDto> Register(UsuarioRegistroRequestDto usuarioRegistroRequestDto) {
    var existeEmail = await _context.Users.Where(x => x.Email == usuarioRegistroRequestDto.Email).AnyAsync();
    if(existeEmail) {
      throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El email ya existe en la base de datos" });
    }

    var existeUserName = await _context.Users.Where(x => x.UserName == usuarioRegistroRequestDto.UserName).AnyAsync();
    if(existeUserName) {
      throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El username ya existe en la base de datos" });
    }

    var usuario = new Usuario {
      Nombre = usuarioRegistroRequestDto.Nombre,
      Apellido = usuarioRegistroRequestDto.Apellido,
      Telefono = usuarioRegistroRequestDto.Telefono,
      Email = usuarioRegistroRequestDto.Email,
      UserName = usuarioRegistroRequestDto.UserName,
    };

    var resultado = await _userManager.CreateAsync(usuario, usuarioRegistroRequestDto.Password);
    if(resultado.Succeeded) {
      return TransformerUserToUserDto(usuario);
    }

    throw new MiddlewareException(HttpStatusCode.InternalServerError, new { mensaje = "Error al registrar el usuario" });
  }
}