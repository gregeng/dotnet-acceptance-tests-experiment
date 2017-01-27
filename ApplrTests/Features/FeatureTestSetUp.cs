using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace ApplrTests.Features
{
    [SetUpFixture]
    public abstract class FeatureTestSetUp<TStartup>
    {
        public static BrowserSession Browser { get; private set; }
        public static Process Server { get; private set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            StartServer();
            SetUpBrowserSession();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            TearDownBrowserSession();
            StopServer();
        }

        protected virtual int Port
        {
            get { return 8080; }
        }
        
        protected virtual SessionConfiguration BrowserConfiguration
        {
            get { return _getBrowserConfiguration(); }
        }

        private SessionConfiguration _getBrowserConfiguration()
        {
            var sessionConfiguration = new SessionConfiguration();
            sessionConfiguration.Driver = typeof(SeleniumWebDriver);
            sessionConfiguration.Browser = Coypu.Drivers.Browser.Chrome;
            sessionConfiguration.AppHost = "localhost";
            sessionConfiguration.Port = Port;
            return sessionConfiguration;
        }
        
        private void StartServer()
        {
            var implAssembly = Assembly.GetAssembly(typeof(TStartup));
            var assemblyName = implAssembly.GetName().Name;

            var testPath = Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var directory = new DirectoryInfo(testPath).Parent.Parent.Parent.Parent;
            var appPath = Path.Combine(directory.FullName, assemblyName);

            var iisPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express",
                "iisexpress.exe");
            var args = $"/path:\"{appPath}\" /port:{Port}";

            Server = Process.Start(iisPath, args);
        }
        
        private void SetUpBrowserSession()
        {
            Browser = new BrowserSession(BrowserConfiguration);
        }
        
        public void TearDownBrowserSession()
        {
            Browser.Dispose();
        }
        
        public void StopServer()
        {
            if (Server.HasExited)
            {
                return;
            }
            Server.CloseMainWindow();
            Server.WaitForExit();
        }
    }
}
