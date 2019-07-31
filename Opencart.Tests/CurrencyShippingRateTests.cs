using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Support.UI;

namespace Opencart.Tests
{
    [TestFixture]
    class CurrencyShippingRateTests
    {
        IWebDriver MyDriver;

        [SetUp]
        public void SetUp()
        {
            MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            ServiceMethodsSet.UserLogIn(MyDriver, "johnsmith@gmail.com", "12121212");
            MyDriver.FindElement(By.XPath("//a[text()='Phones & PDAs']")).Click();
            MyDriver.FindElement(By.CssSelector(".product-layout .fa.fa-shopping-cart")).Click();
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
            MyDriver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement Country = new SelectElement(MyDriver.FindElement(By.Id("input-country")));
            Country.SelectByText("United Kingdom");
            SelectElement Zone = new SelectElement(MyDriver.FindElement(By.Id("input-zone")));
            Zone.SelectByText("Aberdeen");
            MyDriver.FindElement(By.Id("input-postcode")).Clear();
            MyDriver.FindElement(By.Id("input-postcode")).SendKeys("123456");
            MyDriver.FindElement(By.Id("button-quote")).Click();
        }

        [TearDown]
        public void TearDown()
        {
            MyDriver.Quit();
        }

        [Test]
        public void ShippingRateInModalWindowEquielsShippingRateInSummaryTable()
        {
            
            string ShippingMethodLabel = MyDriver.FindElement(By.CssSelector("#modal-shipping label")).Text;
            decimal ActualShippingRate = Decimal.Parse(Regex.Match(ShippingMethodLabel, @"\d*\.\d*").Value.Replace('.', ','));
            MyDriver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            MyDriver.FindElement(By.Id("button-shipping")).Click();
            string ShippingRateInTable = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Flat Shipping Rate')]/parent::td/following-sibling::td")).Text;
            decimal ShippingRateInTableValue = Decimal.Parse(Regex.Match(ShippingRateInTable, @"\d*\.\d*").Value.Replace('.', ','));
            Assert.AreEqual(ActualShippingRate, ShippingRateInTableValue);
        }

        [TestCase("USD", @"^\$\d+\.\d{2}")]
        [TestCase("EUR", @"\d+\.\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.\d{2}")]
        public void ShippingRateInModalWindowHasCorrectCurrencyFormat(string currency, string pattern)
        {
            ServiceMethodsSet.ChangeCurrency(MyDriver, currency);
            string ShippingMethodLabel = MyDriver.FindElement(By.CssSelector("#modal-shipping label")).Text;
            int HyphenIndex = ShippingMethodLabel.IndexOf('-');
            string ShippingRate = ShippingMethodLabel.Substring(HyphenIndex+1).Trim();
            StringAssert.IsMatch(pattern, ShippingRate);

        }
    }
}
