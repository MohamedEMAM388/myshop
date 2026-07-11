using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myShop.BLL.DTOS.Account;

namespace myshop.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager ,
                                      SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model) {

            // validate on loginvm
            if (!ModelState.IsValid)
                return View(model);
            // check if user exist 
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) {

                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

         
            

            var result = await _signInManager.PasswordSignInAsync(User,
                                             model.Password, model.RememberMe, false);

            
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your account is locked out.");
            else if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your account is not allowed.");
            else
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password.");


            return View(model);

        }

        [HttpGet]
        public IActionResult Register() {

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model) { 
        
            if(!ModelState.IsValid)
                return View(model);

            // create application user 
            var usre = new ApplicationUser()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                Address = model.Address,
                City = model.City
            };

            var result = await _userManager.CreateAsync(usre , model.Password);

            if (result.Succeeded) {


                await _userManager.AddToRoleAsync(usre, "Customer");
                // redirect to login page 
                return RedirectToAction(nameof(Login));
            }

            // represnt error 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout() {

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        
        }
    }
}
