using System;
using System.Text;
using NUnit.Framework;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace Opencart.Tests
{
    [TestFixture]
    class CurrencyInShoppingCartTests
    {
        IWebDriver MyDriver;

        [SetUp]
        public void SetUp()
        {
            MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.XPath("//span[contains(text(),'My Account')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            MyDriver.FindElement(By.Id("input-email")).SendKeys("johnsmith@gmail.com");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("12121212" + Keys.Enter);
        }

        [TearDown]
        public void TearDown()
        {
            MyDriver.Quit();
        }

        [TestCase("USD", @"^\$\d+\.?\d{2}")]
        [TestCase("EUR", @"\d+\.?\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.?\d{2}")]
        public void CheckPriceIsInCorrectCurrencyFormat(string currency, string pattern)
        {
            MyDriver.FindElement(By.XPath("//a[text()='Phones & PDAs']")).Click();
            MyDriver.FindElement(By.CssSelector(".product-layout .fa.fa-shopping-cart")).Click();
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            string UnitPrice = MyDriver.FindElements(By.CssSelector("form .table.table-bordered tbody>tr>td.text-right"))[0].Text;
            string Total = MyDriver.FindElements(By.CssSelector("form .table.table-bordered tbody>tr>td.text-right"))[1].Text;
            Console.WriteLine(UnitPrice);
            Console.WriteLine(Total);
            Regex Pattern = new Regex(pattern);
            Assert.IsTrue(Pattern.IsMatch(UnitPrice));
            Assert.IsTrue(Pattern.IsMatch(Total));
        }
    }
}
