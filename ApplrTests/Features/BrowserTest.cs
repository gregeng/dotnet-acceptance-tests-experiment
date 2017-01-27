using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Coypu;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace ApplrTests.Features
{
    [TestFixture]
    public abstract class BrowserTest
    {
        protected abstract Database Database { get; }
        protected BrowserSession Browser {  get { return ApplrFeatureTestSetUp.Browser; } }

        protected BrowserTest()
        {
            ExclusionRules = new List<Predicate<string>>();
            ExcludeTable("__MigrationHistory");
        }

        private List<Predicate<string>> ExclusionRules { get; }

        protected void ExcludeTable(Predicate<string> predicate)
        {
            ExclusionRules.Add(predicate);
        }

        protected void ExcludeTable(string table)
        {
            ExcludeTable(s => s == table);
        }

        protected void ExcludeTable(Regex tableMatcher)
        {
            ExcludeTable(tableMatcher.IsMatch);
        }

        [SetUp]
        public void EnsureServerRunning()
        {
            if (ApplrFeatureTestSetUp.Server.HasExited)
            {
                Assert.Inconclusive("Server terminated unexpectedly.");
            }
        }

        [SetUp]
        public void ClearDatabase()
        {
            var db = Database;
            var conn = db.Connection;
            try
            {
                var tables = new List<string>();
                conn.Open();
                var schema = conn.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row[2].ToString();
                    if (!ExclusionRules.Any(rule => rule.Invoke(tableName)))
                    {
                        tables.Add(tableName);
                    }
                }

                var tx = db.BeginTransaction();
                foreach (string table in tables)
                {
                    db.ExecuteSqlCommand($"DELETE FROM [{table}];");
                }
                tx.Commit();
            }
            finally
            {
                conn.Close();
            }
        }

        
    }
}