using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Teste.Web.Models;
using Teste.Web.Services;

namespace Teste.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AccountController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [EnableRateLimiting("LoginLimiter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var usuario = await _authService.ValidateUserAsync(model.Email, model.Password);

                if (usuario == null)
                {
                    return Json(new { success = false, message = "Email e/ou senha inválidos." });
                }

                //Criando as Claims (informações que vão dentro do cookie)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                var ultimoLogin = usuario.UltimoLogin ?? DateTime.UtcNow;

                HttpContext.Session.SetString("UltimoLogin", ultimoLogin.ToString());

                usuario.UltimoLogin = DateTime.Now;

                await _usuarioService.SalvarAsync(usuario);

                //Agora retorna a URL já resolvida no servidor
                return Json(new { success = true, redirectUrl = Url.Action("index", "home") });
            }
            catch (Exception)
            {
                // Logue o erro de verdade aqui (em arquivo, banco, etc)
                return Json(new { success = false, message = $"Erro interno. Tente novamente mais tarde." });
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["Message"] = "Sua sessão expirou. Faça login novamente.";
                TempData["tipoAlert"] = "warning";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync("CookieAuth");

            TempData["Message"] = "Deslogado com sucesso.";
            TempData["tipoAlert"] = "success";

            return RedirectToAction("Login", "Account");
        }
    }
}