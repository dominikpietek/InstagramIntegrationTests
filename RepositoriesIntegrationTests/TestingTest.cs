using Instagram.Databases;
using Instagram.Models;
using System.Configuration;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class TestingTest : IDisposable
    {
        private readonly InstagramDbContext _dbContext;

        public TestingTest()
        {
            _dbContext = new InstagramDbContext("MainDb");
        }

        [Fact]
        public void YourIntegrationTest()
        {
            if(_dbContext != null)
            {
                _dbContext.Users.Add(new User() 
                { 
                    Nickname = "Dominik",
                    EmailAdress = "",
                    FirstName = "",
                    LastName = "",
                    Password = "",
                    Birthdate = DateTime.Now,
                    ProfilePhoto = new ProfileImage() { }
                });
                _dbContext.SaveChanges();
            }


            User user = _dbContext.Users.First();

            Assert.Equal(user.Nickname, "Dominik");
        }

        public void Truncate()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            //Truncate();
        }
    }
}
