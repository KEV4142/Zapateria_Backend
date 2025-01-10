using System.Net;
using Aplicacion.Interfaces;
using Aplicacion.Tablas.Accounts;
using Aplicacion.Tablas.Accounts.Login;
using Aplicacion.Tablas.Accounts.UsuarioUpdatePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Entidades;
using static Aplicacion.Tablas.Accounts.Login.LoginCommand;
using static Aplicacion.Tablas.Accounts.UsuarioUpdatePassword.UsuarioUpdatePasswordCommand;

namespace WebApi.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserAccessor _user;
    public AccountController(ISender sender, IUserAccessor user)
    {
        _sender = sender;
        _user = user;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginCommandRequest(request);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized(resultado);
    }

    [Authorize(PolicyMaster.USUARIO_CREATE)]
    [HttpPut("password/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Profile>> ActualizarPasswordUsuario(
        [FromBody] UsuarioUpdatePasswordRequest request,
        string id,
        CancellationToken cancellationToken
    )
    {
        var command = new UsuarioUpdatePasswordCommandRequest(request,id);
        var resultado = await _sender.Send(command, cancellationToken);
        return resultado.IsSuccess ? Ok(resultado.Value) : Unauthorized(resultado);
    }
}
