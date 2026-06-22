using InventoryManagement.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Login
{
    public class CurrentUser
    {
        public static UserDto ? User { get; private set; }

        public static bool IsLoggedIn => User != null;

        public static void Login(UserDto user)
        {
            User = user;
        }

        public static void Logout()
        {
            User = null;
        }
    }
}
