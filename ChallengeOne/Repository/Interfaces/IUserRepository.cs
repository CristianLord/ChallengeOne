using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;

namespace ChallengeOne.Repository.Interfaces
{
    /// <summary>
    /// Allows access to methods for manage users
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>A list with all users.</returns>
        Task<IEnumerable<User>> GetAll();

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="idUser">User ID/param>
        /// <returns>A user data</returns>
        Task<User> GetUser(int idUser);

        /// <summary>
        /// Shows all users in database except current one and their subscriptions.
        /// </summary>
        /// <param name="idUser">User ID.</param>
        /// <returns>A list of users without of the user subscriptions.</returns>
        Task<List<UserViewModel>> GetAllWithoutSubscriptions(int idUser);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="idUser">User ID/param>
        /// <returns>A user data</returns>
        Task<UserViewModel> GetViewUser(int idUser);
    }
}