using System.Net;
using Aplicacion.Core;
using Aplicacion.Tablas.Categorias.CategoriaCreate;
using Aplicacion.Tablas.Categorias.CategoriaUpdate;
using Aplicacion.Tablas.Categorias.CategoriaUpdateEstado;
using Aplicacion.Tablas.Categorias.GetCategoria;
using Aplicacion.Tablas.Categorias.GetCategoriasPagin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Entidades;
using static Aplicacion.Tablas.Categorias.CategoriaCreate.CategoriaCreateCommand;
using static Aplicacion.Tablas.Categorias.CategoriaUpdate.CategoriaUpdateCommand;
using static Aplicacion.Tablas.Categorias.CategoriaUpdateEstado.CategoriaUpdateEstadoCommand;
using static Aplicacion.Tablas.Categorias.GetCategoria.GetCategoriaQuery;
using static Aplicacion.Tablas.Categorias.GetCategorias.GetCategoriasQuery;
using static Aplicacion.Tablas.Categorias.GetCategoriasActivos.GetCategoriasActivosQuery;
using static Aplicacion.Tablas.Categorias.GetCategoriasPagin.GetCategoriasPaginQuery;

namespace WebApi.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriasController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(PolicyMaster.CATEGORIA_CREATE)]
    [HttpPost("registro")]
    public async Task<ActionResult<Result<int>>> CategoriaCreate(
        [FromBody] CategoriaCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CategoriaCreateCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_UPDATE)]
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<int>>> CategoriaUpdate(
        [FromBody] CategoriaUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new CategoriaUpdateCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_UPDATE)]
    [HttpPut("estado/{id}")]
    public async Task<ActionResult<Result<int>>> CategoriaUpdateEstado(
        [FromBody] CategoriaUpdateEstadoRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new CategoriaUpdateEstadoCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_READ)]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<CategoriaResponse>> CategoriaGet(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetCategoriaQueryRequest { CategoriaID = id };
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_READ)]
    [HttpGet("listado")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CategoriaResponse>> CategoriasGet(
        CancellationToken cancellationToken
    )
    {
        var query = new GetCategoriasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_READ)]
    [HttpGet("activos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CategoriaResponse>> GetCategoriasActivos(
        CancellationToken cancellationToken
    )
    {
        var query = new GetCategoriasActivasQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado);
    }

    [Authorize(PolicyMaster.CATEGORIA_READ)]
    [HttpGet("paginacion")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<CategoriaResponse>>> PaginationSucursales(
        [FromQuery] GetCategoriasPaginRequest request,
        CancellationToken cancellationToken
    )
    {

        var query = new GetCategoriasPaginQueryRequest { CategoriasPaginRequest = request };
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound(resultado);
    }
}
