namespace ChallengeOne.Models.ViewModels
{
    public class JournalViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile? File { get; set; }
    }
}
