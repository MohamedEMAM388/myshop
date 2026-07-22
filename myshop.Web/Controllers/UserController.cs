using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myShop.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace myshop.PL.Controllers
{
    [Authorize(Policy = "OnlyAdmin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService , UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5;

            var result = await _userService.GetAllUsersAsync(page, pageSize);

            return View(result);
        }

        // GET: User/PromoteToAdmin/5
        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            var result = await _userService.PromoteToAdminAsync(userId);
            if (!result)
            {
                TempData["Error"] = "Failed to promote user to admin.";
            }
            else
            {
                TempData["Success"] = "User promoted to admin successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DemoteToCustomer(string userId)
        {
            var result = await _userService.DemoteToCustomerAsync(userId);
            if (!result)
            {
                TempData["Error"] = "Failed to demote user to customer.";
            }
            else
            {
                TempData["Success"] = "User demoted to customer successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> LockUser(string userId)
        {
            var result = await _userService.LockUserAsync(userId);
            if (!result)
            {
                TempData["Error"] = "Failed to lock user.";
            }
            else
            {
                TempData["Success"] = "User locked successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var result = await _userService.UnlockUserAsync(userId);
            if (!result)
            {
                TempData["Error"] = "Failed to unlock user.";
            }
            else
            {
                TempData["Success"] = "User unlocked successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            // check if the cuurent user id is null
            if(currentUserId is null)
                return Unauthorized();
            var result = await _userService.DeleteUserAsync(userId, currentUserId);
            if (!result)
            {
                TempData["Error"] = "Failed to delete user.";
            }
            else
            {
                TempData["Success"] = "User deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }




    }
}
