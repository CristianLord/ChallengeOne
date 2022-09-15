using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;

namespace ChallengeOne.Repository.Interfaces
{
    /// <summary>
    /// Allows access to methods for manage access
    /// </summary>
    public interface IAccessRepository
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns>If the action was successful</returns>
        Task<bool> RegisterUser(User user);

        /// <summary>
        /// Verify if an email exist in database.
        /// </summary>
        /// <param name="email">Email to verify.</param>
        /// <returns>If the email exist</returns>
        Task<bool> EmailExist(string email);

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="login">Login data.</param>
        /// <returns>User to log in</returns>
        Task<User> LoginUser(Login login);

        /// <summary>
        /// Check if a user exist
        /// </summary>
        /// <param name="login">Login data</param>
        /// <returns>If the user is valid.</returns>
        Task<bool> IsValid(Login login);
    }
}
