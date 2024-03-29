using Instagram.Databases;
using Instagram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.IntegrationTests
{
    public static class TruncateDb
    {
        public static void TruncateAndCreateEssentialData(InstagramDbContext db)
        {
            // truncate
            db.Users.RemoveRange(db.Users);
            db.Posts.RemoveRange(db.Posts);
            db.Tags.RemoveRange(db.Tags);
            db.Comments.RemoveRange(db.Comments);
            db.CommentResponses.RemoveRange(db.CommentResponses);
            db.ProfileImages.RemoveRange(db.ProfileImages);
            db.PostImages.RemoveRange(db.PostImages);
            db.Stories.RemoveRange(db.Stories);
            db.StoryImages.RemoveRange(db.StoryImages);
            db.Friends.RemoveRange(db.Friends);
            db.GotFriendRequestModels.RemoveRange(db.GotFriendRequestModels);
            db.SentFriendRequestModels.RemoveRange(db.SentFriendRequestModels);
            db.Messages.RemoveRange(db.Messages);
            db.SaveChanges();
            // create data
            db.Users.Add(new User()
            {
                Nickname = "admin",
                EmailAdress = "admin",
                FirstName = "admin",
                LastName = "admin",
                Password = "admin",
                Birthdate = DateTime.Now,
                ProfilePhoto = new ProfileImage()
                {
                    ImageBytes = new byte[10]
                },
            });
            db.Users.Add(new User()
            {
                Nickname = "admin",
                EmailAdress = "admin",
                FirstName = "admin",
                LastName = "admin",
                Password = "admin",
                Birthdate = DateTime.Now,
                ProfilePhoto = new ProfileImage()
                {
                    ImageBytes = new byte[10]
                },
            });
            db.SaveChanges();
            int userId = db.Users.First().Id;
            int friendId = db.Users.OrderBy(u => u.Birthdate).Last().Id;
            db.Posts.Add(new Post()
            {
                UserId = userId,
                Description = "desc",
                Location = "loc",
                Image = new PostImage()
                {
                    ImageBytes = new byte[10]
                },
                PublicationDate = DateTime.Now
            });
            db.SaveChanges();
            int postId = db.Posts.First().Id;
            db.Stories.Add(new Story()
            {
                UserId = userId,
                PublicationDate = DateTime.Now,
                Image = new StoryImage()
                {
                    ImageBytes = new byte[10]
                },
            });
            db.Comments.Add(new Comment()
            {
                AuthorId = userId,
                PostId = postId,
                Content = "content",
                PublicationDate = DateTime.Now
            });
            db.Friends.Add(new Friend()
            {
                UserId = userId,
                FriendId = friendId
            });
            db.Stories.Add(new Story()
            {
                UserId = userId,
                PublicationDate = DateTime.Now,
                Image = new StoryImage()
                {
                    ImageBytes = new byte[10]
                }
            });
            db.SaveChanges();
        }
    }
}
