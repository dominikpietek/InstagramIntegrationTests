using Instagram.Databases;
using Instagram.Interfaces;
using Instagram.Models;
using Instagram.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class BothCommentsRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IBothCommentsRepository<Comment> _bothCommentsRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        public BothCommentsRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _bothCommentsRepository = new BothCommentsRepository<Comment>(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
        }
        [Fact]
        public async Task AddCommentAsync_Add_ReturnTrue()
        {
            Comment comment = new Comment()
            {
                AuthorId = _userId,
                PostId = _postId,
                Content = "content",
                PublicationDate = DateTime.Now
            };
            
            bool result = await _bothCommentsRepository.AddCommentAsync(comment);
            
            Assert.Equal(true, result);
        }
        [Fact]
        public async Task DeleteCommentAsync_Delete_ReturnTrue()
        {
            bool result = await _bothCommentsRepository.DeleteCommentAsync(_commentId);
            
            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetCommentAsync_Get_ReturnComment()
        {
            var result = await _bothCommentsRepository.GetCommentAsync(_commentId);

            Assert.IsType(typeof(Comment), result);
        }
        [Fact]
        public async Task GetCommentWithResponsesAsync_Get_ReturnComment()
        {
            var result = await _bothCommentsRepository.GetCommentWithResponsesAsync(_commentId);

            Assert.IsType(typeof(Comment), result);
        }
        [Fact]
        public async Task UpdateCommentAsync_Update_ReturnTrue()
        {
            var comment = await _bothCommentsRepository.GetCommentAsync(_commentId);

            var result = await _bothCommentsRepository.UpdateCommentAsync(comment);

            Assert.Equal(true, result);
        }
    }
}
