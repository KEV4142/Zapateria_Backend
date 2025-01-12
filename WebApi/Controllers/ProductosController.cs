using System.Net;
using Aplicacion.Core;
using Aplicacion.Tablas.Productos.ProductoCreate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Entidades;
using static Aplicacion.Tablas.Productos.ProductoCreate.ProductoCreateCommand;

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
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }
}
