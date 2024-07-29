using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDtos;
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
    return TransformerUserToUserDto(usuario!);
  }

  public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLoginRequestDto) {
    var usuario = await _userManager.FindByEmailAsync(usuarioLoginRequestDto.Email);
    await _signInManager.CheckPasswordSignInAsync(usuario!, usuarioLoginRequestDto.Password!, false);

    return TransformerUserToUserDto(usuario!);
  }

  public async Task<UsuarioResponseDto> Register(UsuarioRegistroRequestDto usuarioRegistroRequestDto) {
    var usuario = new Usuario {
      Nombre = usuarioRegistroRequestDto.Nombre,
      Apellido = usuarioRegistroRequestDto.Apellido,
      Telefono = usuarioRegistroRequestDto.Telefono,
      Email = usuarioRegistroRequestDto.Email,
      UserName = usuarioRegistroRequestDto.UserName,
    };

    await _userManager.CreateAsync(usuario, usuarioRegistroRequestDto.Password);

    return TransformerUserToUserDto(usuario);
  }
}