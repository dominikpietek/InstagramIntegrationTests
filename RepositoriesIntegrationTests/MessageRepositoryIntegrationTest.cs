using Instagram.Databases;
using Instagram.Interfaces;
using Instagram.Models;
using Instagram.Repositories;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Message = Instagram.Models.Message;

namespace Instagram.IntegrationTests.RepositoriesIntegrationTests
{
    public class MessageRepositoryIntegrationTest
    {
        private readonly InstagramDbContext _db;
        private readonly IMessageRepository _messageRepository;
        private readonly int _userId;
        private readonly int _postId;
        private readonly int _commentId;
        private readonly int _friendId;
        public MessageRepositoryIntegrationTest()
        {
            _db = new InstagramDbContext("MainDb");
            TruncateDb.TruncateAndCreateEssentialData(_db);
            _messageRepository = new MessageRepository(_db);
            _userId = _db.Users.First().Id;
            _postId = _db.Posts.First().Id;
            _commentId = _db.Comments.First().Id;
            _friendId = _db.Users.OrderBy(u => u.Birthdate).Last().Id;
        }
        [Fact]
        public async Task AddMessage_Add_ReturnTrue()
        {
            Message message = new Message()
            {
                UserId = _userId,
                FriendId = _friendId,
                SendDate = DateTime.Now,
                Content = "content"
            };

            bool result = await _messageRepository.AddMessage(message);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task GetUserMessagesToFriend_GetMessages_ReturnListMessage()
        {
            var result = await _messageRepository.GetUserMessagesToFriend(_userId, _friendId);

            Assert.IsType(typeof(List<Message>), result);
        }
        [Fact]
        public async Task RemoveMessage_Remove_ReturnTrue()
        {
            Message message = new Message()
            {
                UserId = _userId,
                FriendId = _friendId,
                SendDate = DateTime.Now,
                Content = "content"
            };
            _db.Messages.Add(message);
            _db.SaveChanges();

            bool result = await _messageRepository.RemoveMessage(message);

            Assert.Equal(true, result);
        }
        [Fact]
        public async Task UpdateMessage_Update_ReturnTrue()
        {
            Message message = new Message()
            {
                UserId = _userId,
                FriendId = _friendId,
                SendDate = DateTime.Now,
                Content = "content"
            };
            _db.Messages.Add(message);
            _db.SaveChanges();
            message.Content = "c";

            bool result = await _messageRepository.UpdateMessage(message);

            Assert.Equal(true, result);
        }
    }
}
