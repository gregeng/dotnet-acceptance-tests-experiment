using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applr.Models;

namespace ApplrTests.Features
{
    public class ApplrBrowserTest : BrowserTest
    {
        protected override Database Database {
            get { return GetDatabase(); }
        }

        private DbContext _context;

        private Database GetDatabase()
        {
            _context = _context ?? new PostDbContext();
            return _context.Database;
        }
    }
}
