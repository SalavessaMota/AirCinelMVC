﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;
using System.Collections.Generic;

namespace AirCinelMVC.Helpers
{
    public interface IUserHelper
    {
        IEnumerable<User> GetAllUsers();

        IEnumerable<IdentityRole> GetAllRoles();

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task CheckRoleAsync(string roleName);

        Task<IdentityResult> AddUserToRoleAsync(User user, string role);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<List<string>> GetRolesAsync(User user);

        Task<IdentityResult> RemoveRolesAsync(User user, IEnumerable<string> roles);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

    }
}
