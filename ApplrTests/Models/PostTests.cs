using NUnit.Framework;
using Applr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applr.Models.Tests
{
    [TestFixture()]
    public class PostTests
    {
        [Test()]
        public void IsBadTest()
        {
            Assert.That(new Post().IsBad(), Is.True);
        }
    }
}