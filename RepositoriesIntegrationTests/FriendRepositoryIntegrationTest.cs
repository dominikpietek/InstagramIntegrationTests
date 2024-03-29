using Instagram.Databases;
using Instagram.Interfaces;
using Instagram.Models;
using Instagram.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class FriendRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IFriendRepository _friendRepository;
        private readonly int _userId;
        private readonly int _friendId;
        public FriendRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _friendRepository = new FriendRepository(_db);
            _userId = _db.Users.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task GetFriendAsync_Get_ReturnFriend()
        {
            var result = await _friendRepository.GetFriendAsync(_userId, _friendId);

            Assert.IsType(typeof(Friend), result);
        }
        [Fact]
        public async Task AddFriendAsync_Add_ReturnTrue()
        {
            bool result = await _friendRepository.AddFriendAsync(_friendId, _userId);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetAllUserFriendsIdAsync_GetAll_ReturnFriendIds()
        {
            var result = await _friendRepository.GetAllUserFriendsIdAsync(_userId);

            Assert.IsType(typeof(List<int>), result);
        }
        [Fact]
        public async Task GetFriendId_GetId_ReturnId()
        {
            int result = await _friendRepository.GetFriendId(_userId, _friendId);

            Assert.NotEqual(0, result);
        }
        [Fact]
        public async Task IsFriend_IsFriends_ReturnTrue()
        {
            bool result = await _friendRepository.IsFriend(_userId, _friendId);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task RemoveFriendAsync_Remove_ReturnTrue()
        {
            await _friendRepository.AddFriendAsync(_friendId, _userId);
            bool result = await _friendRepository.RemoveFriendAsync(_friendId, _userId);

            Assert.Equal(true, result);
        }
    }
}
