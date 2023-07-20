using Microsoft.AspNetCore.Identity;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Helpers
{
    public interface IUserHelper

    {
        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(User user);


        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<User> GetUserAsync(Guid userId);

        Task<User> GetUserAsync(string email);

        Task<User> AddUserAsync(AddUserViewModel model, Guid imageId);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

    }
}
