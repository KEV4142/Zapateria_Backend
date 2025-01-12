using System.Net;
using Aplicacion.Core;
using Aplicacion.Tablas.Productos.ProductoCreate;
using Aplicacion.Tablas.Productos.ProductoUpdate;
using Aplicacion.Tablas.Productos.ProductoUpdateEstado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Entidades;
using static Aplicacion.Tablas.Productos.ProductoCreate.ProductoCreateCommand;
using static Aplicacion.Tablas.Productos.ProductoUpdate.ProductoUpdateCommand;
using static Aplicacion.Tablas.Productos.ProductoUpdateEstado.ProductoUpdateEstadoCommand;

namespace WebApi.Controllers;
[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly ISender _sender;

    public ProductosController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(PolicyMaster.PRODUCTOS_CREATE)]
    [HttpPost("registro")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Result<int>>> ProductoCreate(
        [FromForm] ProductoCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ProductoCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_UPDATE)]
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<int>>> ProductoUpdate(
        [FromBody] ProductoUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new ProductoUpdateCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_UPDATE)]
    [HttpPut("estado/{id}")]
    public async Task<ActionResult<Result<int>>> ProductoUpdateEstado(
        [FromBody] ProductoUpdateEstadoRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new ProductoUpdateEstadoCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }
}
