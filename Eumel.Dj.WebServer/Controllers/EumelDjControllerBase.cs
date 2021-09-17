using Eumel.Dj.Core.Models;
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
                _ = _tokenService.TryFindUser(Token, out var username);
                return username ?? string.Empty;
            }
        }

        protected string Token => Request.Headers.ContainsKey(Constants.UserToken) ? Request.Headers[Constants.UserToken][0] : string.Empty;

        protected string GetClientIp()
        {
            return Request.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
}