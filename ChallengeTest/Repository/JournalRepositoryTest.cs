using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChallengeOne.Test.Repository
{
    public class JournalRepositoryTest
    {
        private async Task<DatabaseContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DatabaseContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Journals.CountAsync() == 0)
            {
                databaseContext.Users.Add(
                    new User()
                    {
                        Id = 1,
                        FirstName = "Cristian",
                        LastName = "Vazquez",
                        Email = "cristian@gmail.com",
                        Password = "password"
                    });

                databaseContext.Journals.Add(
                    new Journal()
                    {
                        Title = "Test",
                        File = new byte[]{ 2,3,4,5},
                        IdUser = 1,
                        UploadDate = DateTime.Now
                    });

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }

        [Fact]
        public async Task JournalRepository_GetByIdUser_Ok()
        {
            //Arrange
            int idUser = 1;
            var database = await GetDatabaseContext();
            var journalRepository = new JournalRepository(database);

            //Act
            var result = await journalRepository.GetByIdUser(idUser);

            //Assert
            Assert.IsType<List<JournalViewModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task JournalRepository_Create_Ok()
        {
            //Arrange
            int idUser = 1;
            var journal = new JournalViewModel()
            {
                Title = "Test"
            };
            var database = await GetDatabaseContext();
            var journalRepository = new JournalRepository(database);

            //Act
            var result = await journalRepository.Create(idUser, journal);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task JournalRepository_GetById()
        {
            //Arrange
            int journalId = 1;
            var database = await GetDatabaseContext();
            var journalRepository = new JournalRepository(database);

            //Act
            var result = await journalRepository.GetById(journalId);

            //Assert
            Assert.IsType<Journal>(result);
        }
    }
}
