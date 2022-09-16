using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChallengeOne.Test.Repository
{
    /// <summary>
    /// Verify some functionality of the Subscription repository.
    /// </summary>
    public class UserRepositoryTest
    {

        private async Task<DatabaseContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DatabaseContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Users.CountAsync() == 0)
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

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }

        //If all users is succesfully obtained.
        [Fact]
        public async Task UserRepository_GetAll_Ok()
        {
            //Arrange
            var database = await GetDatabaseContext();
            var userRepository = new UserRepository(database);

            //Act
            var result = await userRepository.GetAll();

            //Assert
            Assert.IsType<List<User>>(result);
            Assert.NotNull(result);
        }

        //If a users is succesfully obtained.
        [Fact]
        public async Task UserRepository_GetUser_Ok()
        {
            //Arrange
            int idUser = 1;
            var database = await GetDatabaseContext();
            var userRepository = new UserRepository(database);

            //Act
            var result = await userRepository.GetUser(idUser);

            //Assert
            Assert.IsType<User>(result);
            Assert.NotNull(result);
        }

        //If a users is succesfully obtained to view.
        [Fact]
        public async Task UserRepository_GetViewUser_Ok()
        {
            //Arrange
            int idUser = 1;
            var database = await GetDatabaseContext();
            var userRepository = new UserRepository(database);

            //Act
            var result = await userRepository.GetViewUser(idUser);

            //Assert
            Assert.IsType<UserViewModel>(result);
            Assert.NotNull(result);
        }

        //If all users without subscriptions is succesfully obtained.
        [Fact]
        public async Task UserRepository_GetAllWithoutSubscriptions_Ok()
        {
            //Arrange
            int idUser = 1;
            var database = await GetDatabaseContext();
            var userRepository = new UserRepository(database);

            //Act
            var result = await userRepository.GetAllWithoutSubscriptions(idUser);

            //Assert
            Assert.IsType<List<UserViewModel>>(result);
            Assert.NotNull(result);
        }
    }
}
