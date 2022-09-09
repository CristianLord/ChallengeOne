using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeOne.Models
{
    public class Journal
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[]? File { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public DateTime UploadDate { get; set; }

        public virtual User? User { get; set; }
    }
}
