using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;

namespace ChallengeOne.Repository.Interfaces
{
    /// <summary>
    /// Allows access to methods for manage subscriptions
    /// </summary>
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Subscribe a user to another.
        /// </summary>
        /// <param name="idUser">Id of the user to be subscribed.</param>
        /// <param name="idSubscribeTo">ID of the user to subscribe.</param>
        /// <returns>If the action was successful.</returns>
        Task<bool> SubscribeTo(int idUser, int idSubscribeTo);

        /// <summary>
        /// Shows user's subscriptions.
        /// </summary>
        /// <param name="idUser">User ID.</param>
        /// <returns>A list of the user subscriptions.</returns>
        Task<List<UserViewModel>> GetAllSubscriptions(int idUser);

        /// <summary>
        /// Unsubscribed a user to other.
        /// </summary>
        /// <param name="idSusbcription">ID of the subcription.</param>
        /// <param name="idUser">ID of the user to unsubcribe.</param>
        /// <returns>If the action was succesful.</returns>
        Task<bool> Unsubscription(int idSusbcription, int idUser);
    }
}