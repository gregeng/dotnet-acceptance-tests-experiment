using System.Data.Entity;
using System.Linq;
using Applr;
using Applr.Models;
using Coypu;
using NUnit.Framework;

namespace ApplrTests.Features
{
    [TestFixture]
    public class PostTests : ApplrBrowserTest
    {
        [Test]
        public void ValidatePresenceTest()
        {
            Browser.Visit("/posts");
            Assert.That(Browser.HasContent("Index"));
            Assert.That(Browser.FindCss("a[href='/posts/Create']", "Create New").Exists(), Is.True,
                "Expected 'Create New' link");
            
            Assert.That(Browser.FindLink("Edit", Options.First).Exists(), Is.False, "Did not expect to find an 'Edit Link' since there should be no existing Posts");
            Assert.That(Browser.FindLink("Details", Options.First).Exists(), Is.False, "Did not expect to find a 'Details Link' since there should be no existing Posts");
            Assert.That(Browser.FindLink("Delete", Options.First).Exists(), Is.False, "Did not expect to find a 'Delete Link' since there should be no existing Posts");
        }

        [Test]
        public void CreatePostTest()
        {
            Browser.Visit("/posts");
            Browser.ClickLink("Create New");
            Assert.That(Browser.HasContent("Create"));

            Browser.FillIn("Content").With("I just made a post!");
            Browser.ClickButton("Create");

            Assert.That(Browser.HasContent("Index"));
            Assert.That(Browser.HasContent("I just made a post!"));

            Assert.That(Browser.FindLink("Edit", Options.First).Exists(), Is.True);
            Assert.That(Browser.FindLink("Details", Options.First).Exists(), Is.True);
            Assert.That(Browser.FindLink("Delete", Options.First).Exists(), Is.True);

            Assert.That(new PostDbContext().Posts.Count(), Is.EqualTo(1));
        }


        [Test]
        public void ValidatePostContentTest()
        {
            Browser.Visit("/posts");
            Browser.ClickLink("Create New");
            Assert.That(Browser.HasContent("Create"));

            Browser.FillIn("Content").With("Is");
            Browser.ClickButton("Create");

            Assert.That(Browser.Location.PathAndQuery, Does.Contain("Create"));
            Assert.That(Browser.HasContent("The field Content must be a string with a minimum length of 3 and a maximum length of 60."));

            Browser.FillIn("Content").With("");
            Browser.ClickButton("Create");

            Assert.That(Browser.Location.PathAndQuery, Does.Contain("Create"));
            Assert.That(Browser.HasContent("The Content field is required."));

            Browser.FillIn("Content").With("Is this field valid even though the length of the characters in it are over the maximum length of sixty characters?");
            Browser.ClickButton("Create");

            Assert.That(Browser.Location.PathAndQuery, Does.Contain("Create"));
            Assert.That(Browser.HasContent("The field Content must be a string with a minimum length of 3 and a maximum length of 60."));
        }

        [Test]
        public void DeletePostTest()
        {
            AddPosts("Seeded Post");
            Browser.Visit("/posts");

            Assume.That(Browser.HasContent("Seeded Post"));

            Browser.ClickLink("Delete");

            Assert.That(Browser.HasContent("Delete"));
            Assert.That(Browser.Location.PathAndQuery, Does.Contain("Delete"));

            Browser.ClickButton("Delete");

            Assume.That(Browser.HasContent("Index"));
            Assert.That(Browser.HasContent("Seeded Post"), Is.False);
        }

        [Test]
        public void UpdatePostTest()
        {
            AddPosts("Rough Draft Post Name");
            Browser.Visit("/posts");

            Assume.That(Browser.HasContent("Rough Draft Post Name"));

            Browser.ClickLink("Edit");

            Assert.That(Browser.HasContent("Edit"));
            Assert.That(Browser.Location.PathAndQuery, Does.Contain("Edit"));

            Browser.FillIn("Content").With("Final Draft Post Name");
            Browser.ClickButton("Save");

            Assume.That(Browser.HasContent("Index"));
            Assert.That(Browser.HasContent("Final Draft Post Name"), Is.True);
        }

        private void AddPosts(params string[] bodies)
        {
            var ctx = new PostDbContext();
            foreach (string body in bodies)
            {
                ctx.Posts.Add(new Post{Content = body});
            }
            ctx.SaveChanges();
        }
    }
}