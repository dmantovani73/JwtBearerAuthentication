using JwtBearerAuthentication.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtBearerAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        readonly UserManager<IdentityUser> _userManager;

        readonly JwtConfiguration _jwtConfiguration;

        public JwtController(UserManager<IdentityUser> userManager, JwtConfiguration jwtConfiguration)
        {
            _userManager = userManager;
            _jwtConfiguration = jwtConfiguration;
        }

        [HttpPost]
        public async Task<ActionResult<string>> GenerateToken(GenerateTokenRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            // Cerco l'utente per UserName.
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return BadRequest();

            // Verifico che la password sia corretta.
            if (!await _userManager.CheckPasswordAsync(user, request.Password)) return BadRequest();

            // Genero un nuovo token JWT.
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var token = new JwtSecurityToken(
                    issuer: _jwtConfiguration.ValidIssuer,
                    audience: _jwtConfiguration.ValidAudience,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
            });
        }

        public class GenerateTokenRequest
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}
