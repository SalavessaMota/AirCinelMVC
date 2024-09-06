using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AirCinelMVC.Data.Entities;

 
namespace AirCinelMVC.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);


        Task<IdentityResult> AddUserAsync(User user, string password);



    }
}
