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
public class InmuebleController : ControllerBase
{
  private readonly IInmuebleRepository _inmuebleRepository;
  private readonly IMapper _mapper;

  public InmuebleController(IInmuebleRepository inmuebleRepository, IMapper mapper)
  {
    _inmuebleRepository = inmuebleRepository;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetInmuebles()
  {
    var inmuebles = await _inmuebleRepository.GetAllInmuebles();
    var inmueblesResponse = _mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles);
    return Ok(inmueblesResponse);
  }

  [HttpGet("{id}", Name = "GetInmuebleById")]
  public async Task<ActionResult<InmuebleResponseDto>> GetInmuebleById(int id)
  {
    var inmueble = await _inmuebleRepository.GetInmuebleById(id);
    if (inmueble is null)
    {
      throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = $"No se encontró el inmueble por este id {id}" });
    };

    var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmueble);
    return Ok(inmuebleResponse);
  }

  [HttpPost]
  public async Task<ActionResult<InmuebleResponseDto>> CreateInmueble([FromBody] InmuebleRequestDto inmuebleRequest)
  {
    var inmuebleModel = _mapper.Map<Inmueble>(inmuebleRequest);
    await _inmuebleRepository.CreateInmueble(inmuebleModel);
    await _inmuebleRepository.SaveChanges();

    var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmuebleModel);
    return CreatedAtRoute(nameof(GetInmuebleById), new { id = inmuebleResponse.Id }, inmuebleResponse);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteInmueble(int id)
  {
    var inmueble = await _inmuebleRepository.GetInmuebleById(id);
    if (inmueble is null)
    {
      throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = $"No se encontró el inmueble por este id {id}" });
    };

    await _inmuebleRepository.DeleteInmueble(inmueble.Id);
    await _inmuebleRepository.SaveChanges();
    return Ok();
  }
}