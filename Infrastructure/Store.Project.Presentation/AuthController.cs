using Microsoft.AspNetCore.Mvc;
using Store.Project.Services.Abstractions;
using Store.Project.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
          var result = await  _serviceManager.AuthService.LoginAsync(loginRequest);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest )
        {
            var result = await _serviceManager.AuthService.RegisterAsync(registerRequest);
            return Ok(result);
        }

    }
}
