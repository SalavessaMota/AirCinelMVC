using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;


namespace AirCinelMVC.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);


        Task<IdentityResult> AddUserAsync(User user, string password);


        Task<SignInResult> LoginAsync(LoginViewModel model);


        Task LogoutAsync();
    }
}
