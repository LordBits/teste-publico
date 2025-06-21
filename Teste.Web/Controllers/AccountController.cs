using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Models;
using Teste.Web.Services.Interfaces;

namespace Teste.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _authService.ValidateUserAsync(model.Email, model.Password);

            if (usuario == null)
            {
                return Json(new { success = false, message = "Email e/ou senha inválidos." });
            }

            // Criando as Claims (informações que vão dentro do cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Se quiser que a sessão dure mesmo se fechar o navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Tempo de expiração
            };

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

             // Agora retorna a URL já resolvida no servidor
            return Json(new { success = true, redirectUrl = Url.Action("index", "home") });
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["Message"] = "Sua sessão expirou. Faça login novamente.";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync("CookieAuth");

            Response.Cookies.Delete(".AspNetCore.Cookies");

            TempData["Message"] = "Deslogado com sucesso.";

            return RedirectToAction("Login");
        }
    }
}