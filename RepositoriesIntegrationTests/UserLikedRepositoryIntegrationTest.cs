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
    public class UserLikedRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IUserLikedRepository _userLikedRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        public UserLikedRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _userLikedRepository = new UserLikedRepository(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task AddLikeAsync_Add_ReturnTrue()
        {
            UserLiked userLiked = new UserLiked()
            {
                UserThatLikedId = _userId,
                LikedThing = Enums.LikedThingsEnum.Comment,
                LikedThingId = _postId
            };

            bool result = await _userLikedRepository.AddLikeAsync(userLiked);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task IsLikedBy_IsLiked_ReturnFalse()
        {
            bool result = await _userLikedRepository.IsLikedBy(_userId, Enums.LikedThingsEnum.CommentResponse, 3);

            Assert.Equal(false, result);
        }
        [Fact]
        public async Task RemoveLikeAsync_Remove_ReturnTrue()
        {
            UserLiked userLiked = new UserLiked()
            {
                UserThatLikedId = _userId,
                LikedThing = Enums.LikedThingsEnum.Comment,
                LikedThingId = _postId
            };
            _db.UsersLiked.Add(userLiked);
            _db.SaveChanges();

            bool result = await _userLikedRepository.RemoveLikeAsync(_userId, Enums.LikedThingsEnum.Comment, _postId);

            Assert.Equal(true, result);
        }
    }
}
