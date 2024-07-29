using NetKubernetes.Dtos.UsuarioDtos;

namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepository {
  Task<UsuarioResponseDto> GetUsuario();
  Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLoginRequestDto);
  Task<UsuarioResponseDto> Register(UsuarioRegistroRequestDto usuarioRegistroRequestDto);
}