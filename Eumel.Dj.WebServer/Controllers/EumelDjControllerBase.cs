using Microsoft.AspNetCore.Mvc;

namespace Eumel.Dj.WebServer.Controllers
{
    public class EumelDjControllerBase : ControllerBase
    {
        public string Usertoken => Request.Headers.ContainsKey("usertoken") ? Request.Headers["usertoken"][0] : string.Empty;
    }
}