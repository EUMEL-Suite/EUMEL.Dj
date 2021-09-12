using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eumel.Dj.WebServer.Controllers
{
    public class EumelDjControllerBase : ControllerBase
    {
        private readonly ITokenService _tokenService;

        protected EumelDjControllerBase(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected string Username
        {
            get
            {
                var token = Request.Headers.ContainsKey("usertoken") ? Request.Headers["usertoken"][0] : string.Empty;
                _ = _tokenService.TryFindUser(token, out var username);
                return username ?? string.Empty;
            }
        }

        protected string GetClientIp()
        {
            return Request.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
}