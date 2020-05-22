using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CW2.DAL;
using CW2.Models;
using CW2.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CW2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDbService _dbService;

        public AuthController(IConfiguration configuration, IDbService dbService)
        {
            _configuration = configuration;
            _dbService = dbService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            var authenticatedStudent = _dbService.GetUserInfo(request.Login);
            if (authenticatedStudent == null 
                || !PasswordHelper.CheckPassword(authenticatedStudent.PasswordHash, authenticatedStudent.PasswordSalt, request.Password))
            {
                return BadRequest("Wrong login or password");
            }

            var refreshToken = Guid.NewGuid().ToString();
            
            _dbService.SaveRefreshToken(authenticatedStudent.IndexNumber, refreshToken);

            return CreateTokenResponse(authenticatedStudent, refreshToken);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshRequestDTO refresh)
        {
            var authenticatedStudent = _dbService.UseRefreshToken(refresh.RefreshToken);
            if (authenticatedStudent == null)
            {
                return BadRequest("Wrong refresh token");
            }

            var refreshToken = Guid.NewGuid().ToString();

            _dbService.SaveRefreshToken(authenticatedStudent.IndexNumber, refreshToken);

            return CreateTokenResponse(authenticatedStudent, refreshToken);
        }

        private IActionResult CreateTokenResponse(UserInfo authenticatedStudent, string refreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, authenticatedStudent.IndexNumber),
                new Claim(ClaimTypes.Name, $"{authenticatedStudent.FirstName} {authenticatedStudent.LastName}"),
                new Claim(ClaimTypes.Role, Roles.Employee)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshToken
            });
        }
    }
}
