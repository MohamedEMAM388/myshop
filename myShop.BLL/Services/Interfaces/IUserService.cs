using Microsoft.Extensions.Options;
using myShop.BLL.DTOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Interfaces
{
    public interface IUserService
    {

        // get all users => rerturn user dto 
        public Task<PaginatedList<UserDTO>> GetAllUsersAsync(int page, int pageSize);

        //        Role Management
        //Allow administrators to:

        //Promote Customer → Admin
        //Demote Admin → Customer

        public Task<bool> PromoteToAdminAsync(string userId);
        public Task<bool> DemoteToCustomerAsync(string userId);

        // lock and unlock user accounts
        public Task<bool> LockUserAsync(string userId);
        public Task<bool> UnlockUserAsync(string userId);

        //        Delete Users(Optional)
        //Allow administrators to delete users safely.

        //Prevent administrators from deleting their own accounts.

        public Task<bool> DeleteUserAsync(string userId, string currentUserId);

    }
}
