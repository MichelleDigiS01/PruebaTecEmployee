using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BL.Login _login;

        public LoginController(BL.Login login)
        {
            _login = login;
        }

        //Post frombody user y password

        [HttpPost]
        [Route("GetUserByEmail")]
        public IActionResult GetUserByEmail([FromBody] ML.Login login)
        {
            ML.Result result = _login.GetUserByEmail(login);

            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;

                string token = GenerateJwtToken(usuario);
                result.Object = token;

                return Ok(result);

            }
            else
            {
                return BadRequest(result);
            }
        }


        private string GenerateJwtToken(ML.Usuario usuario)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Role, usuario.Rol.Name),
            new Claim(ClaimTypes.Name, usuario.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ht1ejtthghXbl1KdOwyRzLUDDfiQhGsbZMiGrguet3BRBjz37RvGa"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
