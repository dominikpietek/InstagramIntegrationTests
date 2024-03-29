using Azure.Core;
using Instagram.Databases;
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
    public class GotSentFriendRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IGotSentFriendRequestModelRepository _gotRequestRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        public GotSentFriendRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _gotRequestRepository = new GotSentFriendRequestModelRepository<GotFriendRequestModel>(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task AddAsync_Add_ReturnTrue()
        {
            var request = new GotFriendRequestModel()
            {
                UserId = _userId,
                StoredUserId = _friendId
            };

            bool result = await _gotRequestRepository.AddAsync(request);

            Assert.Equal(true, result);
            _db.GotFriendRequestModels.Remove(request);
            _db.SaveChanges();
        }
        [Fact]
        public async Task GetAllAsync_GetAll_ReturnFriendRequests()
        {
            var result = await _gotRequestRepository.GetAllAsync(_userId);

            Assert.IsType(typeof(List<int>), result);
        }
        [Fact]
        public async Task IsRequest_IsRequested_ReturnFalse()
        {
            bool result = await _gotRequestRepository.IsRequest(_userId, _friendId);

            Assert.Equal(false, result);
        }
        [Fact]
        public async Task RemoveAsync_Remove_ReturnTrue()
        {
            _db.GotFriendRequestModels.Add(new GotFriendRequestModel()
            {
                UserId = _userId,
                StoredUserId = _friendId
            });
            _db.SaveChanges();

            bool result = await _gotRequestRepository.RemoveAsync(_userId, _friendId);

            Assert.Equal(true, result);
        }
    }
}
