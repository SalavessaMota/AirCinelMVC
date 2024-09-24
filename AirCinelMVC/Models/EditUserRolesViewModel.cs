using System.Collections.Generic;

namespace AirCinelMVC.Models
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<UserRoleViewModel> Roles { get; set; }
    }
}
