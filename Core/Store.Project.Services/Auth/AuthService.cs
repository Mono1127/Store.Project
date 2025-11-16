using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Project.Domain.Entities.Identity;
using Store.Project.Domain.Exceptions;
using Store.Project.Domain.Exceptions.BadRequest;
using Store.Project.Domain.Exceptions.Unauthorized;
using Store.Project.Services.Abstractions.Auth;
using Store.Project.Shared;
using Store.Project.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Auth
{
    public class AuthService(UserManager<AppUser> userManager,IOptions<JwtOptions> options, IMapper mapper) : IAuthService
    {
        public async Task<bool> CheckEmailExistAsync(string email)
        {
          return await userManager.FindByEmailAsync(email) != null;

        }

        public async Task<UserResponse?> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) throw new UserNotFoundException(email);
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };

        }

        public async Task<AddressDto?> GetCurrentUserAddressAsync(string email)
        {
           var user = await userManager.Users.Include(U => U.address).FirstOrDefaultAsync(U => U.Email.ToLower() == email.ToLower());
            if (user == null) throw new UserNotFoundException(email);
            return mapper.Map<AddressDto>(user.address);
        }

        
        public async Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto address, string email)
        {
            var user = await userManager.Users.Include(U => U.address).FirstOrDefaultAsync(U => U.Email.ToLower() == email.ToLower());
            if (user == null) throw new UserNotFoundException(email);

            if (user.address == null)
            {
                user.address = mapper.Map<Address>(address);
            }
            else
            {
                user.address.FristName = address.FristName;
                user.address.LastName = address.LastName;
                user.address.Street = address.Street;
                user.address.City = address.City;
                user.address.Country = address.Country;
            }
           await userManager.UpdateAsync(user);

            return mapper.Map<AddressDto>(user.address);
        }

        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new UserNotFoundException(request.Email);
           var flag = await  userManager.CheckPasswordAsync(user, request.Password);
            if (!flag) throw new UnauthorizedException();
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber,
            };
          var result = await userManager.CreateAsync(user,request.Password);

          if (!result.Succeeded) throw new RegisterationBadRequestException(result.Errors.Select(E =>E.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };

        }

        private async Task<string> GenerateTokenAsync(AppUser appUser)
        {

            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.GivenName, appUser.DisplayName),
                new Claim (ClaimTypes.Email, appUser.Email),
                new Claim (ClaimTypes.MobilePhone, appUser.PhoneNumber),
            };
            var roles = await userManager.GetRolesAsync(appUser);
            foreach (var role in roles) 
            { 
                authClaims.Add(new Claim (ClaimTypes.Role, role));
            }

            var jwtOptions = options.Value;
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.Now.AddDays(jwtOptions.DurationDays),
                signingCredentials: new SigningCredentials(key , SecurityAlgorithms.HmacSha256)

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
