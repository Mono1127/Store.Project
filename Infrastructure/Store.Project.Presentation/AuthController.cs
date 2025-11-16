using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Project.Services.Abstractions;
using Store.Project.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpGet("EmailExists")]
        public async Task<IActionResult> EmailExists( string email)
        {
            var result = await _serviceManager.AuthService.CheckEmailExistAsync(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrnetUser()
        {
           var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.GetCurrentUserAsync(email.Value);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<IActionResult> GetCurrnetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.GetCurrentUserAddressAsync(email.Value);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("UpdateCurrentUserAddress")]
        public async Task<IActionResult> UpdateCurrnetUserAddress(AddressDto address)
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.UpdateCurrentUserAddressAsync(address,email.Value);
            return Ok(result);
        }






    }
}
