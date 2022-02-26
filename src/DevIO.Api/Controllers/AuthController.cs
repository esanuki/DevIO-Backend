using DevIO.Api.Interop.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(
            INotificadorService notificador, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse();
            }

            foreach (var item in result.Errors) NotificarErro(item.Description);

            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserViewModel login)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, true);

            if (result.Succeeded)
            {
                NotificarErro("Deu certo!");
                return CustomResponse(login);
            }
            if (result.IsLockedOut)
            {
                NotificarErro("Usuario temporariamente bloqueado pr tentativas inválidas");
                return CustomResponse();
            }

            NotificarErro("Usuário ou senha incorretos");
            return CustomResponse();
        }
    }

    
}
