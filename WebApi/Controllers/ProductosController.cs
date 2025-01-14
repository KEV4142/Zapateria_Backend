using System.Net;
using Aplicacion.Core;
using Aplicacion.Tablas.Productos.GetProducto;
using Aplicacion.Tablas.Productos.GetProductosPagin;
using Aplicacion.Tablas.Productos.ProductoCreate;
using Aplicacion.Tablas.Productos.ProductoUpdate;
using Aplicacion.Tablas.Productos.ProductoUpdateEstado;
using Aplicacion.Tablas.Productos.ProductoUpdateImagen;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Entidades;
using static Aplicacion.Tablas.Productos.GetProducto.GetProductoQuery;
using static Aplicacion.Tablas.Productos.GetProductos.GetProductosQuery;
using static Aplicacion.Tablas.Productos.GetProductosActivos.GetProductosActivosQuery;
using static Aplicacion.Tablas.Productos.GetProductosPagin.GetProductosPaginQuery;
using static Aplicacion.Tablas.Productos.ProductoCreate.ProductoCreateCommand;
using static Aplicacion.Tablas.Productos.ProductoUpdate.ProductoUpdateCommand;
using static Aplicacion.Tablas.Productos.ProductoUpdateEstado.ProductoUpdateEstadoCommand;
using static Aplicacion.Tablas.Productos.ProductoUpdateImagen.ProductoUpdateImagenCommand;

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
    [Authorize(Policy = PolicyMaster.IMAGEN_CREATE)]
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
    [Authorize(PolicyMaster.PRODUCTOS_UPDATE)]
    [Authorize(Policy = PolicyMaster.IMAGEN_UPDATE)]
    [HttpPut("imagen/{id}")]
    public async Task<ActionResult<Result<int>>> ProductoUpdateImagen(
        [FromForm] ProductoUpdateImagenRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new ProductoUpdateImagenCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductoResponse>> ProductoGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductoQueryRequest { ProductoID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ProductoResponse>> ProductosGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductosQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ProductoResponse>> GetProductosActivos(
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductosActivasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.PRODUCTOS_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<ProductoResponse>>> PaginationProductos(
        [FromQuery] GetProductosPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetProductosPaginQueryRequest { ProductosPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound(resultado);
    }
}
