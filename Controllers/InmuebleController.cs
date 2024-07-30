using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Inmuebles;
using NetKubernetes.Dtos.InmuebleDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InmuebleController : ControllerBase {
  private readonly IInmuebleRepository _inmuebleRepository;
  private readonly IMapper _mapper;

  public InmuebleController(IInmuebleRepository inmuebleRepository, IMapper mapper) {
    _inmuebleRepository = inmuebleRepository;
    _mapper = mapper;
  }

  [HttpGet]
  public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmuebles() {
    var inmuebles = _inmuebleRepository.GetAllInmuebles();
    var inmueblesResponse = _mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles);
    return Ok(inmueblesResponse);
  }

  [HttpGet("{id}", Name = "GetInmuebleById")]
  public ActionResult<InmuebleResponseDto> GetInmuebleById(int id) {
    var inmueble = _inmuebleRepository.GetInmuebleById(id);
    if (inmueble is null) {
      throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = $"No se encontró el inmueble por este id {id}" });
    };

    var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmueble);
    return Ok(inmuebleResponse);
  }

  [HttpPost]
  public ActionResult<InmuebleResponseDto> CreateInmueble([FromBody] InmuebleRequestDto inmuebleRequest) {
    var inmuebleModel = _mapper.Map<Inmueble>(inmuebleRequest);
    _inmuebleRepository.CreateInmueble(inmuebleModel);
    _inmuebleRepository.SaveChanges();

    var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmuebleModel);
    return CreatedAtRoute(nameof(GetInmuebleById), new { id = inmuebleResponse.Id }, inmuebleResponse);
  }

  [HttpDelete("{id}")]
  public ActionResult DeleteInmueble(int id) {
    var inmueble = _inmuebleRepository.GetInmuebleById(id);
    if (inmueble is null) {
      throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = $"No se encontró el inmueble por este id {id}" });
    };

    _inmuebleRepository.DeleteInmueble(inmueble.Id);
    _inmuebleRepository.SaveChanges();
    return NoContent();
  }
}