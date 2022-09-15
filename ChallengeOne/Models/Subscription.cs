using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChallengeOne.Models
{
    /// <summary>
    /// Subscription model.
    /// </summary>
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? IdUser { get; set; }
        [ForeignKey("SubscribedTo")]
        public int? IdSubscribedUser { get; set; }

        public virtual User? User { get; set; }
        public virtual User? SubscribedTo { get; set; }
    }
}
