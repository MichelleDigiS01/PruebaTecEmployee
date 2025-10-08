using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PL.Controllers
{
    public class LoginController : Controller
    {

        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(ML.Login login)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7180/api/Login/GetUserByEmail", login);

            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<ML.Result>();

                if (apiResult.Correct)
                {

                    string token2 = apiResult.Object?.ToString()?.Trim('"');

                    //var tokenProperty = apiResult.Object.GetType().GetProperty("Token");
                    //var token = tokenProperty?.GetValue(apiResult.Object)?.ToString();

                    if (!string.IsNullOrEmpty(token2))
                    {
                        // Guardar el token en una cookie segura
                        Response.Cookies.Append("JwtToken", token2, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                        });

                        return RedirectToAction("GetAll", "Employee");
                    }


                }

            }


            return View(login);

        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Login", "Login");
        }
    }
}
