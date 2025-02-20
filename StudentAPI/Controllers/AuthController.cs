using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly StudentDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(StudentDBContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        // POST api/<AuthController>
        //[HttpPost]
        //public string LoginReq([FromBody] Login loginModel)
        //{
        //    var result = Login(loginModel);
        //    return result;
        //}
        [HttpPost("login")]
        public string Login([FromBody] Login loginModel)
        {
            if (loginModel.Username != null && loginModel.Password != null)
            {
                var user = _context.Users.SingleOrDefault(s => s.Email == loginModel.Username && s.Password == loginModel.Password);
                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.Name),
                        new Claim("Email", user.Email)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("user is not valid");
                }
            }
            else
            {
                throw new Exception("credentials are not valid");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var data = await _context.Users.ToListAsync();
            return Ok(data);
        }
    }
}
