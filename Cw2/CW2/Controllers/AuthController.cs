using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CW2.DAL;
using CW2.Models;
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
            var authenticatedStudent = _dbService.AuthenticateStudent(request.Login, request.Password);
            if (authenticatedStudent == null)
            {
                return BadRequest("Wrong login or password");
            }

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
                refreshToken = Guid.NewGuid()
            });
        }
    }
}
