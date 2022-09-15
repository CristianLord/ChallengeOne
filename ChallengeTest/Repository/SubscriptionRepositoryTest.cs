using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeOne.Test.Repository
{
    public class SubscriptionRepositoryTest
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

                databaseContext.Users.Add(
                    new User()
                    {
                        Id = 2,
                        FirstName = "Bruno",
                        LastName = "Gress",
                        Email = "bruno@gmail.com",
                        Password = "password"
                    });

                databaseContext.Subscriptions.Add(
                    new Subscription()
                    {
                        Id = 1,
                        IdUser = 1,
                        IdSubscribedUser = 2
                    });

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }

        [Fact]
        public async Task SubscriptionRepository_SubscriptionTo_Ok()
        {
            //Arrange
            int idUser = 1;
            int idSubscription = 2;
            var database = await GetDatabaseContext();
            var subscriptionRepository = new SubscriptionRepository(database);

            //Act
            var result = await subscriptionRepository.SubscribeTo(idUser, idSubscription);

            //Assert
            Assert.True(result);
        }
    }
}
