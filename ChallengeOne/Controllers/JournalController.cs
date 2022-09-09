using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    public class JournalController : Controller
    {
        private readonly DatabaseContext _database;

        public JournalController(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IActionResult> Index()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<JournalViewModel> list = await (from journal in _database.Journals
                                                 where journal.IdUser == idUser
                                                 select new JournalViewModel
                                                 {
                                                     Id = journal.Id,
                                                     Title = journal.Title
                                                 }).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: JournalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JournalViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var journal = new Journal();

                if (viewModel.File != null)
                {
                    if (viewModel.File.Length > 0 && viewModel.File.Length < 500000)
                    {
                        using (var target = new MemoryStream())
                        {
                            viewModel.File.CopyTo(target);
                            //viewModel.Journal.File = target.ToArray();
                            journal.File = target.ToArray();
                            journal.IdUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            journal.Title = viewModel.Title;
                            journal.UploadDate = DateTime.Now;

                            await _database.Journals.AddAsync(journal);
                            await _database.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                    }
                }

                return View(viewModel);
            }
            catch
            {
                ViewBag.Message = "Something went wrong.";
                return View(viewModel);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var journal = await _database.Journals.Where(journal => journal.Id == id).FirstOrDefaultAsync();
            if (journal == null)
                return NotFound();
            return View(journal);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var journal = await _database.Journals.Where(journal => journal.Id == id).FirstOrDefaultAsync();
            if (journal == null)
                return NotFound();

            _database.Journals.Remove(journal);
            await _database.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// See someone else's journals
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public async Task<IActionResult> SeeJournal(int idUser)
        {
            List<JournalViewModel> list = await (from journal in _database.Journals
                                                 where journal.IdUser == idUser
                                                 select new JournalViewModel
                                                 {
                                                     Id = journal.Id,
                                                     Title = journal.Title
                                                 }).ToListAsync();
            return View(list);
        }

    }
}
