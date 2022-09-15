using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ChallengeOne.Repository
{
    ///<inheritdoc/>
    public class JournalRepository : IJournalRepository
    {

        private readonly DatabaseContext _database;

        public JournalRepository(DatabaseContext database)
        {
            _database = database;
        }

        
        public async Task<List<JournalViewModel>> GetByIdUser(int idUser)
        {
            List<JournalViewModel> list = await(from journal in _database.Journals
                                                where journal.IdUser == idUser
                                                select new JournalViewModel
                                                {
                                                    Id = journal.Id,
                                                    Title = journal.Title
                                                }).ToListAsync();

            return list;
        }

        public async Task<bool> Create(int idUser, JournalViewModel viewModel)
        {
            try
            {
                if (idUser == 0)
                    return false;

                using (var target = new MemoryStream())
                {
                    var journal = new Journal();
                    viewModel.File.CopyTo(target);
                    journal.File = target.ToArray();
                    journal.IdUser = idUser;
                    journal.Title = viewModel.Title;
                    journal.UploadDate = DateTime.Now;

                    await _database.Journals.AddAsync(journal);
                    return await _database.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<Journal> GetById(int id)
        {
            var journal = await _database.Journals.Where(journal => journal.Id == id).FirstOrDefaultAsync();
            return journal;
        }

        
        public async Task<JournalViewModel> GetViewById(int id)
        {
            var journal = await (from j in _database.Journals
                                               where j.Id == id
                                               select new JournalViewModel
                                               {
                                                   Id = j.Id,
                                                   Title = j.Title
                                               }).FirstOrDefaultAsync();
            return journal;
        }

        public async Task<bool> Update(int idUser, JournalViewModel viewModel)
        {
            try
            {
                using (var target = new MemoryStream())
                {
                    viewModel.File.CopyTo(target);
                    Journal journal = new();
                    journal.File = target.ToArray();
                    journal.Id = viewModel.Id;
                    journal.IdUser = idUser;
                    journal.Title = viewModel.Title;
                    journal.UploadDate = DateTime.Now;

                    _database.Journals.Update(journal);
                    await _database.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                var journal = await _database.Journals.Where(journal => journal.Id == id).FirstOrDefaultAsync();
                if (journal == null)
                    return false;
                _database.Journals.Remove(journal);
                return await _database.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
