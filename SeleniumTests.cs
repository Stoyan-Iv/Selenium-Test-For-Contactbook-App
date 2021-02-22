using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace SeleniumTestForExam
{
    public class SeleniumTests
    {
        RemoteWebDriver driver;
        const string baseUrl = "https://contactbook.stoyaniv.repl.co";
        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        [Test]
        public void AssertFirstContactIsSteveJobs()
        {
            driver.Navigate().GoToUrl(baseUrl);

            var viewContacts = driver.FindElementByCssSelector("body > main > div > a:nth-child(1)");
            viewContacts.Click();

            var firstContactFirstName = driver.FindElementByCssSelector("#contact1 > tbody > tr.fname > td");
            var firstContactLastName = driver.FindElementByCssSelector("#contact1 > tbody > tr.lname > td");

            Assert.AreEqual("Steve", firstContactFirstName.Text);
            Assert.AreEqual("Jobs", firstContactLastName.Text);
        }
        [Test]
        public void FindContactByKeywordAndAssertItIsFirst()
        {
            driver.Navigate().GoToUrl(baseUrl);
            var searchContactButton = driver.FindElementByCssSelector("body > aside > ul > li:nth-child(4) > a");
            searchContactButton.Click();
            var searchFeild = driver.FindElementByCssSelector("#keyword");
            Thread.Sleep(1000);
            searchFeild.SendKeys("albert");

            var searchButton = driver.FindElementByCssSelector("#search");
            searchButton.Click();

            var firstContactFirstName = driver.FindElementByCssSelector("#contact3 > tbody > tr.fname > td");
            var firstContactLastName = driver.FindElementByCssSelector("#contact3 > tbody > tr.lname > td");


            Assert.AreEqual("Albert", firstContactFirstName.Text);
            Assert.AreEqual("Einstein", firstContactLastName.Text);
        }

        [Test]
        public void FindContactByInvalidKeyword()
        {
            driver.Navigate().GoToUrl(baseUrl);
            var searchContactButton = driver.FindElementByCssSelector("body > aside > ul > li:nth-child(4) > a");
            searchContactButton.Click();
            var searchFeild = driver.FindElementByCssSelector("#keyword");
            Thread.Sleep(1000);
            searchFeild.SendKeys("invalid2635");

            var searchButton = driver.FindElementByCssSelector("#search");
            searchButton.Click();

            var systemMassage = driver.FindElementByCssSelector("#searchResult");
            Assert.AreEqual("No contacts found.", systemMassage.Text);
        }

        [Test]
        public void CreatContactWithInvalidData()
        {
            driver.Navigate().GoToUrl(baseUrl);
            var creatContactButton = driver.FindElementByCssSelector("body > main > div > a:nth-child(2)");
            creatContactButton.Click();

            Thread.Sleep(1000);
            var firstNameField = driver.FindElementByCssSelector("#firstName");
            var lastNameField = driver.FindElementByCssSelector("#lastName");
            var emailField = driver.FindElementByCssSelector("#email");
            var phoneField = driver.FindElementByCssSelector("#phone");
            var commentsField = driver.FindElementByCssSelector("#comments");
            var createButton = driver.FindElementByCssSelector("#create");

            firstNameField.SendKeys("1234");
            lastNameField.SendKeys("@@@");
            emailField.SendKeys("nomail");
            phoneField.SendKeys("-----");
            commentsField.SendKeys("Good luck at exam!");
            createButton.Click();

            var errorMassage = driver.FindElementByCssSelector("body > main > div");
            Assert.AreEqual("Error: Invalid email!", errorMassage.Text);
        }

        [Test]
        public void CreatContactAndAssertItDesplayed()
        {
            driver.Navigate().GoToUrl(baseUrl);
            var creatContactButton = driver.FindElementByCssSelector("body > main > div > a:nth-child(2)");
            creatContactButton.Click();      
            
            Thread.Sleep(1000);
            var firstNameField = driver.FindElementByCssSelector("#firstName");
            var lastNameField = driver.FindElementByCssSelector("#lastName");
            var emailField = driver.FindElementByCssSelector("#email");
            var phoneField = driver.FindElementByCssSelector("#phone");
            var commentsField = driver.FindElementByCssSelector("#comments");
            var createButton = driver.FindElementByCssSelector("#create");

            firstNameField.SendKeys("Stoyan91");
            lastNameField.SendKeys("Ivanov91");
            emailField.SendKeys("stoian.@abv.bg");
            phoneField.SendKeys("+359 888 333 444");
            commentsField.SendKeys("Good luck at exam!");
            createButton.Click();

            var allCells = driver.FindElementsByCssSelector("table tbody td");
            foreach (var cell in allCells)
            {
                if (cell.Text == "Stoyan91")
                {
                    Assert.AreEqual("Stoyan91", cell.Text);
                }
            }
            Assert.AreEqual("Stoyan91", allCells[allCells.Count - 5].Text);
        }
        [OneTimeTearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}