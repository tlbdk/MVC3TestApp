using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using System.Diagnostics;

namespace TestApp.Tests.Controllers
{

    [TestFixture]
    class MainSeleniumTest
    {
        IWebDriver driver;

        [SetUp]
	    public void Setup()
	    {
            // Link to fix this driver:  http://www.tomdupont.net/2011/10/use-internetexplorerdriver-for.html
		    //driver = new InternetExplorerDriver();
            driver = new FirefoxDriver();
	    }
	    
	    [TearDown]
	    public void Teardown()
	    {
		    driver.Quit();
	    }

        [Test]
        public void TestSearchGoogleForTheAutomatedTester()
        {
            //Navigate to the site
            driver.Navigate().GoToUrl("http://www.google.co.uk");
            //Find the Element and create an object so we can use it
            IWebElement queryBox = driver.FindElement(By.Name("q"));
            //Work with the Element that's on the page
            queryBox.SendKeys("The Automated Tester");
            queryBox.SendKeys(Keys.ArrowDown);
            queryBox.Submit();
            Debug.WriteLine("hello");
            Console.WriteLine(driver.Title);
            //Check that the Title is what we are expecting
            Assert.True(driver.Title.IndexOf("Google") > -1);
        }
    }
}
