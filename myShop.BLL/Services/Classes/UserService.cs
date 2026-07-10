using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myShop.BLL.DTOS;
using myShop.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager ,
                           IMapper mapper) 
        {
            _userManager = userManager;
            _mapper = mapper;
        }



        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    CurrentRole = roles.FirstOrDefault() ?? "No Role",
                    IsLocked = user.LockoutEnd.HasValue &&
                               user.LockoutEnd.Value > DateTimeOffset.UtcNow
                });
            }

            return userDtos;
        }

        public async Task<bool> PromoteToAdminAsync(string userId)
        {
            // get user by id
            var user = await _userManager.FindByIdAsync(userId);
            // check if user exists
            if (user is null)
                return false;
            // get role of the user 
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin")) 
                return false; // user is already an admin

            if (roles.Contains("Customer")) {

                // remove user from customer role
                var removeResult = await _userManager.RemoveFromRoleAsync(user, "Customer");
                if (!removeResult.Succeeded)
                    return false;

            }
            // add user to admin role
            var addResult = await _userManager.AddToRoleAsync(user, "Admin");
            return addResult.Succeeded;
        }

        public async Task<bool> DemoteToCustomerAsync(string userId)
        {
            // Get user by id
            var user = await _userManager.FindByIdAsync(userId);

            // Check if user exists
            if (user is null)
                return false;

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // User is already a Customer
            if (roles.Contains("Customer"))
                return false;

            // Remove Admin role
            var removeResult = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (!removeResult.Succeeded)
                return false;

            // Add Customer role
            var addResult = await _userManager.AddToRoleAsync(user, "Customer");

            return addResult.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId, string currentUserId)
        {
            // check if the user is trying to delete themselves
            if (userId == currentUserId)
                return false;
            var user = await _userManager.FindByIdAsync(userId);
            // check if user exists
            if (user is null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;

        }

        public async Task<bool> LockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ;
           if (user is null)
                return false;
           if(await _userManager.IsLockedOutAsync(user))
                return false;

            var lockAccount = await _userManager.SetLockoutEndDateAsync(user ,DateTimeOffset.UtcNow.AddDays(5));
            return lockAccount.Succeeded;
        }

        public async Task<bool> UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            if (!await _userManager.IsLockedOutAsync(user))
                return false;

            var unlockAccount = await _userManager.SetLockoutEndDateAsync(user,null);
            return unlockAccount.Succeeded;
        }
    }
}
