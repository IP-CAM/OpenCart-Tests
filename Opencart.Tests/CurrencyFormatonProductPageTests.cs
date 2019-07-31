using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace Opencart.Tests
{
    [TestFixture]
    class CurrencyFormatOnProductPageTests
    {
        IWebDriver MyDriver;
        [OneTimeSetUp]
        public void Setup()
        {
            MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.CssSelector(".owl-wrapper-outer")).Click();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            MyDriver.Quit();
        }

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        public void CheckPriceIsPositiveNumber(string currency)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);
            decimal ActualResult = ServiceMethodsSet.GetPriceValue(MyDriver);
            Assert.IsTrue(ActualResult >= 0);
        }

        [TestCase("USD", @"^\$\d+\.?\d{2}")]
        [TestCase("EUR", @"\d+\.?\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.?\d{2}")]
        public void CheckCurrencyFormatOfPrice(string currency, string pattern)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);
            string Price = MyDriver.FindElement(By.XPath("//div[@id='content']//div[@class='col-sm-4']//li/h2")).Text.Trim();
            StringAssert.IsMatch(pattern, Price);
        }

        [TestCase("USD", @"^\$\d+\.?\d{2}")]
        [TestCase("EUR", @"\d+\.?\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.?\d{2}")]
        public void CheckCurrencyFormatOfOldPrice(string currency, string pattern)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);

        }

        [TestCase("USD", @"^\$\d+\.?\d{2}")]
        [TestCase("EUR", @"\d+\.?\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.?\d{2}")]
        public void CheckCurrencyFormatOfExTax(string currency, string pattern)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);
            string ExTaxAllText = MyDriver.FindElement(By.XPath("//div[@id='content']//div[@class='col-sm-4']//li/h2/../following-sibling::li[contains(text(), 'Ex Tax')]")).Text;
            int SemicolumnIndex = ExTaxAllText.IndexOf(":");
            string ExTax = ExTaxAllText.Substring(SemicolumnIndex + 1).Trim();
            Console.WriteLine(ExTax);
            StringAssert.IsMatch(pattern, ExTax);
        }
    }
}
