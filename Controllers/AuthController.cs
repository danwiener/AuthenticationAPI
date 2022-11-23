using Authentication.DTO;
using Microsoft.AspNetCore.Mvc;
using Authentication.Models;
using Authentication.Data;
using Authentication.Services;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private FFContextDb db;
        private IConfiguration configuration;
        public AuthController(IConfiguration configuration, FFContextDb db)
        {
            this.configuration = configuration;
            this.db = db;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                return Unauthorized("Passwords do not match!");
            }
            User user = new User()
            {
                Username = dto.Username,
                Name = dto.Name,
                Email = dto.Email,
                Password = HashService.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(user);
        } // End method

        // Generates access/refresh tokens and cookie which will be stored in the browser for 7 days
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            User? user = db.Users.Where(u => u.Email == dto.Email).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid credentials!");
            }
            if (HashService.HashPassword(dto.Password) != user.Password)
            {
                return Unauthorized("Invalid credentials!");
            }
            string accessToken = TokenService.CreateAccessToken(user.UserId, configuration.GetSection("JWT:AccessKey").Value);
            string refreshToken = TokenService.CreateRefreshToken(user.UserId, configuration.GetSection("JWT:RefreshKey").Value);

            CookieOptions cookieOptions = new();
            cookieOptions.HttpOnly = true;
            Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);

            UserToken token = new()
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiredAt = DateTime.Now.AddDays(7)
            };

            db.UserTokens.Add(token);
            db.SaveChanges();

            return Ok(new
            {
                token = accessToken
            });
        } // End method

        [HttpGet("user")]
        public new IActionResult User()
        {
            string? authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader is null || authorizationHeader.Length <= 8)
            {
                return Unauthorized("Unauthenticated");
            }

            string accessToken = authorizationHeader[7..];

            int id = TokenService.DecodeToken(accessToken, out bool hasTokenExpired);

            if (hasTokenExpired)
            {
                return Unauthorized("Unauthenticated");
            }

            User? user = db.Users.Where(u => u.UserId== id).FirstOrDefault();

            if (user is null)
            {
                return Unauthorized("Unauthenticated!");
            }

            return Ok(user);
        } // End method

        // Successfully returned authenticated user, but access token only lives for 30 seconds. Generate new access token using refresh token
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (Request.Cookies["refresh_token"] is null)
            {
                return Unauthorized("Unauthenticated!");
            }

            string? refreshToken = Request.Cookies["refresh_token"];

            int id = TokenService.DecodeToken(refreshToken, out bool hasTokenExpired);

            if (!db.UserTokens.Where(u => u.UserId == id && u.Token == refreshToken && u.ExpiredAt > DateTime.Now).Any())
            {
                return Unauthorized("Unauthenticated!");
            }

            if (hasTokenExpired)
            {
                return Unauthorized("Unauthenticated!");
            }
            string accessToken = TokenService.CreateAccessToken(id, configuration.GetSection("JWT:AccessKey").Value);

            return Ok(new
            {
                token = accessToken
            });
        } // End method

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string? refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken is null)
            {
                return Ok("Already logged out!");
            }
            db.UserTokens.Remove(db.UserTokens.Where(u => u.Token == refreshToken).First());
            db.SaveChanges();

            Response.Cookies.Delete("refresh_token");

            return Ok("Loged out successfully!");
        }
    } // End class
} // End namespace
