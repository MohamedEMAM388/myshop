using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.DTOS
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string CurrentRole { get; set; } = null!;

        public bool IsLocked { get; set; }


    }
}
