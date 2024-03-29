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
    public class StoryRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IStoryRepository _storyRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        private readonly int _storyId;
        public StoryRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _storyRepository = new StoryRepository(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
            _storyId = _db.Stories.First().Id;
        }
        [Fact]
        public async Task GetAllStoriesAsync_GetAll_ReturnListStories()
        {
            var result = await _storyRepository.GetAllStoriesAsync();

            Assert.IsType(typeof(List<Story>), result);
        }
        [Fact]
        public async Task GetStoryAsync_Get_ReturnStory()
        {
            var result = await _storyRepository.GetStoryAsync(_storyId);

            Assert.IsType(typeof(Story), result);
        }
        [Fact]
        public async Task RemoveStoryAsync_Remove_ReturnTrue()
        {
            Story story = new Story()
            {
                UserId = _userId,
                PublicationDate = DateTime.Now,
                Image = new StoryImage()
                {
                    ImageBytes = new byte[10]
                }
            };
            _db.Stories.Add(story);
            _db.SaveChanges();
            int myStoryId = _db.Stories.OrderBy(s => s.PublicationDate).Last().Id;

            bool result = await _storyRepository.RemoveStoryAsync(myStoryId);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task UpdateStoryAsync_Update_ReturnTrue()
        {
            Story story = _db.Stories.First();

            bool result = await _storyRepository.UpdateStoryAsync(story);

            Assert.Equal(true, result);

        }
        [Fact]
        public async Task AddStoryAsync_Add_ReturnTrue()
        {
            Story story = new Story()
            {
                UserId = _userId,
                PublicationDate = DateTime.Now,
                Image = new StoryImage()
                {
                    ImageBytes = new byte[10]
                }
            };

            bool result = await _storyRepository.AddStoryAsync(_userId, story);

            Assert.Equal(true, result);
        }
    }
}
