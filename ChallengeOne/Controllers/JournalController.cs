using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    public class JournalController : Controller
    {
        private readonly IJournalRepository _journalRepository;

        public JournalController(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        /// <summary>
        /// Shows all journal for the current user.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (idUser == 0)
                    return NotFound();

                var list = await _journalRepository.GetByIdUser(idUser);
                if (list == null)
                    return NotFound();

                return View(list);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        public IActionResult Create() => View();

        /// <summary>
        /// Create a new journal
        /// </summary>
        /// <param name="viewModel">Journal data.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JournalViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(viewModel);

                if (viewModel.File != null)
                {
                    if (viewModel.File.Length > 0 && viewModel.File.Length < 500000)
                    {
                        if(await _journalRepository.Create(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), viewModel))
                            return RedirectToAction("Index");
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

        /// <summary>
        /// Shows a journal details
        /// </summary>
        /// <param name="id">Journal ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id == 0)
                    return NotFound();

                var journal = await _journalRepository.GetById(id);
                if (journal == null)
                    return NotFound();

                System.IO.File.WriteAllBytes("wwwroot/File.pdf", journal.File);
                return View(journal);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Goes to view to edit a journal.
        /// </summary>
        /// <param name="id">Journal ID.</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id == 0)
                    return NotFound();

                var journal = await _journalRepository.GetViewById(id);
                if (journal == null)
                    return NotFound();

                return View(journal);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Edit a journal.
        /// </summary>
        /// <param name="viewModel">Journal data.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JournalViewModel viewModel)
        {
            
            try
            {
                if (!ModelState.IsValid)
                return View(viewModel);

                if (viewModel.File != null)
                {
                    if (viewModel.File.Length > 0 && viewModel.File.Length < 500000)
                    {
                        if (await _journalRepository.Update(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), viewModel))
                            return RedirectToAction("Index");
                    }
                    ViewBag.Error = "File is too heavy.";
                }
                ViewBag.Error = "File is empty";
                return View(viewModel);
            }
            catch
            {
                ViewBag.Message = "Something went wrong.";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Remove a journal by ID.
        /// </summary>
        /// <param name="id">Journal ID.</param>
        /// <returns></returns>
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                if (id == 0)
                    return NotFound();

                if (!(await _journalRepository.Remove(id)))
                    return NotFound();

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// See someone else's journals
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public async Task<IActionResult> SeeJournal(int idUser)
        {
            //List<JournalViewModel> list = await (from journal in _database.Journals
            //                                     where journal.IdUser == idUser
            //                                     select new JournalViewModel
            //                                     {
            //                                         Id = journal.Id,
            //                                         Title = journal.Title
            //                                     }).ToListAsync();
            try
            {
                if (idUser == 0)
                    return NotFound();

                var list = await _journalRepository.GetByIdUser(idUser);
                if (list == null)
                    return NotFound();

                return View(list);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

    }
}
