using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuarioDtos;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase {
  private readonly IUsuarioRepository _usuarioRepository;

  public UsuarioController(IUsuarioRepository usuarioRepository) {
    _usuarioRepository = usuarioRepository;
  }

  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] UsuarioLoginRequestDto request) {
    return await _usuarioRepository.Login(request);
  }

  [AllowAnonymous]
  [HttpPost("registrar")]
  public async Task<ActionResult<UsuarioResponseDto>> Registrar([FromBody] UsuarioRegistroRequestDto request) {
    return await _usuarioRepository.Register(request);
  }

  [HttpGet]
  public async Task<ActionResult<UsuarioResponseDto>> GetUsuario() {
    return await _usuarioRepository.GetUsuario();
  }
}