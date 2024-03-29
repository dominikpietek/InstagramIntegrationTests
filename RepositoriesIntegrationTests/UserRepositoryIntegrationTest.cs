using Instagram.Databases;
using Instagram.DTOs;
using Instagram.Interfaces;
using Instagram.Models;
using Instagram.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class UserRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IUserRepository _userRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        public UserRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _userRepository = new UserRepository(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task GetAllUsersWithPhotosAndRequestsAsync_GetAllUsers_ReturnListUsers()
        {
            var result = await _userRepository.GetAllUsersWithPhotosAndRequestsAsync();

            Assert.IsType(typeof(List<User>), result);
        }
        [Fact]
        public async Task GetUserWithPhotoAndRequestsAsync_GetUser_ReturnUser()
        {
            var result = await _userRepository.GetUserWithPhotoAndRequestsAsync(_userId);

            Assert.IsType(typeof(User), result);
        }
        [Fact]
        public async Task RemoveUserAsync_Remove_ReturnTrue()
        {
            User user = new User()
            {
                Nickname = "d",
                EmailAdress = "admin",
                FirstName = "admin",
                LastName = "admin",
                Password = "admin",
                Birthdate = DateTime.Now,
                ProfilePhoto = new ProfileImage()
                {
                    ImageBytes = new byte[10]
                },
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            int myUserId = _db.Users.Where(u => u.Nickname == "d").First().Id;

            bool result = await _userRepository.RemoveUserAsync(myUserId);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task UpdateUserAsync_Update_ReturnTrue()
        {
            User user = _db.Users.First();
            user.Nickname = "nick";

            bool result = await _userRepository.UpdateUserAsync(user);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task AddUserAsync_Add_ReturnTrue()
        {
            User user = new User()
            {
                Nickname = "Dominik",
                EmailAdress = "admin",
                FirstName = "admin",
                LastName = "admin",
                Password = "admin",
                Birthdate = DateTime.Now,
                ProfilePhoto = new ProfileImage()
                {
                    ImageBytes = new byte[10]
                },
            };

            bool result = await _userRepository.AddUserAsync(user);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetUserByNicknameWithoutIncludesAsync_GetUser_ReturnUser()
        {
            var result = await _userRepository.GetUserByNicknameWithoutIncludesAsync("admin");

            Assert.IsType(typeof(User), result);
        }
        [Fact]
        public async Task GetUserByEmailWithoutIncludesAsync_GetUser_ReturnUser()
        {
            var result = await _userRepository.GetUserByEmailWithoutIncludesAsync("admin");

            Assert.IsType(typeof(User), result);
        }
        [Fact]
        public async Task GetOnlyEssentialDataAsync_GetUser_ReturnUser()
        {
            var result = await _userRepository.GetOnlyEssentialDataAsync(_userId);

            Assert.IsType(typeof(User), result);
        }
        [Fact]
        public async Task GetUsersIdAndNickname_GetUserDto_ReturnListSearchUserDto()
        {
            var result = await _userRepository.GetUsersIdAndNickaname();

            Assert.IsType(typeof(List<SearchUserDto>), result);
        }
    }
}
