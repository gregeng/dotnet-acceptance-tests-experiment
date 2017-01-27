using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applr;
using Applr.Models;
using NUnit.Framework;

namespace ApplrTests.Features
{
    [SetUpFixture]
    public class ApplrFeatureTestSetUp : FeatureTestSetUp<Startup>
    {
    }
}
