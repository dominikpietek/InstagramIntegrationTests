using Instagram.Databases;
using Instagram.Interfaces;
using Instagram.Models;
using Instagram.Repositories;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class PostRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IPostRepository _postRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        public PostRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _postRepository = new PostRepository(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task AddPostAsync_Add_ReturnTrue()
        {
            Post post = new Post()
            {
                UserId = _userId,
                Description = "desc",
                Location = "loc",
                Image = new PostImage()
                {
                    ImageBytes = new byte[1]
                },
                PublicationDate = DateTime.Now
            };

            bool result = await _postRepository.AddPostAsync(post);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetAllPostsWithAllDataToShowAsync_GetPosts_ReturnListPosts()
        {
            var result = await _postRepository.GetAllPostsWithAllDataToShowAsync();

            Assert.IsType(typeof(List<Post>), result);
        }
        [Fact]
        public async Task GetPostWithAllDataAsync_GetPost_ReturnPost()
        {
            var result = await _postRepository.GetPostWithAllDataAsync(_postId);

            Assert.IsType(typeof(Post), result);
        }
        [Fact]
        public async Task GetUserPostsWithAllDataToShow_GetUserPosts_ReturnListPost()
        {
            var result = await _postRepository.GetUserPostsWithAllDataToShowAsync(_userId);

            Assert.IsType(typeof(List<Post>), result);
        }
        [Fact]
        public async Task RemovePostbyIdAsync_Remove_ReturnTrue()
        {
            Post post = new Post()
            {
                UserId = _userId,
                Description = "customDesc",
                Location = "loc",
                Image = new PostImage()
                {
                    ImageBytes = new byte[1]
                },
                PublicationDate = DateTime.Now
            };
            _db.Posts.Add(post);
            _db.SaveChanges();
            int thatPostId = _db.Posts.Where(p => p.Description == "customDesc").First().Id;

            var result = await _postRepository.RemovePostByIdAsync(thatPostId);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task UpdatePostAsync_Update_ReturnTrue()
        {
            Post post = _db.Posts.First();
            post.Location = "l";

            var result = await _postRepository.UpdatePostAsync(post);

            Assert.Equal(true, result);
        }
    }
}
