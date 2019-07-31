using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace Opencart.Tests
{
    [TestFixture]
    class CurrencyLabelButtonTests
    {
        IWebDriver MyDriver;

        [OneTimeSetUp]
        public void Setup()
        {
            MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            MyDriver.Quit();
        }

        [TestCase("USD", @"^\$ Currency")]
        [TestCase("EUR", @"^€ Currency")]
        [TestCase("GBP", @"^\£ Currency")]
        public void CheckCurrencyLabel(string currency, string pattern)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);
            string Actual = MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Text;
            StringAssert.IsMatch(pattern, Actual);
        }
    }
}
