using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applr;
using Applr.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApplrTests.Features
{
    [TestFixture()]
    public class CommentTests : ApplrBrowserTest
    {
        [Test]
        public void CreateTest()
        {
            AddPostWithComments("Hey YouTube, this is my posting walkthrough");
            Browser.Visit("/comments/Create");

            Browser.Select("Hey YouTube, this is my posting walkthrough").From("Post");
            Browser.FillIn("Content").With("speling");

            Browser.ClickButton("Create");

            Assert.That(new PostDbContext().Comments.Count(), Is.EqualTo(1));
        }

        private void AddPostWithComments(string postBody, params string[] commentBodies)
        {
            var post = new Post {Content = postBody};

            IEnumerable<Comment> comments = commentBodies.Select(b => new Comment {Content = b, Post = post});
            
            var postDbContext = new PostDbContext();
            postDbContext.Posts.Add(post);
            postDbContext.Comments.AddRange(comments);
            postDbContext.SaveChanges();
        }
    }
}
