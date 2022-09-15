using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;

namespace ChallengeOne.Repository.Interfaces
{
    /// <summary>
    /// Allows access to methods for manage journals
    /// </summary>
    public interface IJournalRepository
    {
        /// <summary>
        /// Get all journals of a user.
        /// </summary>
        /// <param name="idUser">User id</param>
        /// <returns>A list of all user journals</returns>
        Task<List<JournalViewModel>> GetByIdUser(int idUser);

        /// <summary>
        /// Get a journal by id.
        /// </summary>
        /// <param name="id">Journal ID</param>
        /// <returns>A journal view model.</returns>
        Task<JournalViewModel> GetViewById(int id);

        /// <summary>
        /// Get a journal by ID.
        /// </summary>
        /// <param name="id">Journal ID</param>
        /// <returns>A journal.</returns>
        Task<Journal> GetById(int id);

        /// <summary>
        /// Create a new journal.
        /// </summary>
        /// <param name="idUser">Journal user ID.</param>
        /// <param name="viewModel">Journal data.</param>
        /// <returns>If the action was succesful.</returns>
        Task<bool> Create(int idUser, JournalViewModel viewModel);

        /// <summary>
        /// Update a journal.
        /// </summary>
        /// /// <param name="idUser">Journal user ID.</param>
        /// <param name="viewModel">Journal data.</param>
        /// <returns></returns>
        Task<bool> Update(int idUser, JournalViewModel viewModel);

        /// <summary>
        /// Remove a journal by ID.
        /// </summary>
        /// <param name="id">Journal ID.</param>
        /// <returns></returns>
        Task<bool> Remove(int id);

    }
}