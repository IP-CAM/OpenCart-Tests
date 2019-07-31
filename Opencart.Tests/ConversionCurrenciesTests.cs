using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace Opencart.Tests
{
    [TestFixture]
    class ConversionCurrenciesTests
    {
        decimal EurRate;
        decimal GbpRate;
        decimal UsdPrice;  
        decimal EurPrice;
        decimal GbpPrice;     

        [OneTimeSetUp]
        public void Setup()
        {
            IWebDriver MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            ServiceMethodsSet.AdminLogIn(MyDriver, "admin", "Lv414_Taqc");
            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Localisation')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Currencies')]")).Click();

            EurRate = Decimal.Parse(MyDriver.FindElement(By.XPath("//td[text()='EUR']/following-sibling::td[@class='text-right'][count(*)=0]")).Text.Replace('.', ','));
            GbpRate = Decimal.Parse(MyDriver.FindElement(By.XPath("//td[text()='GBP']/following-sibling::td[@class='text-right'][count(*)=0]")).Text.Replace('.', ','));

            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.CssSelector(".owl-wrapper-outer")).Click();
            ServiceMethodsSet.ChangeCurrency(MyDriver, "USD");
            UsdPrice = ServiceMethodsSet.GetPriceValue(MyDriver);
            ServiceMethodsSet.ChangeCurrency(MyDriver, "EUR");
            EurPrice = ServiceMethodsSet.GetPriceValue(MyDriver);
            ServiceMethodsSet.ChangeCurrency(MyDriver, "GBP");
            GbpPrice = ServiceMethodsSet.GetPriceValue(MyDriver);
            MyDriver.Quit();
        }

        [Test]
        public void CheckEurConversion()
        {
            decimal Conversion =Math.Round(UsdPrice * EurRate, 2);
            Assert.AreEqual(Conversion, EurPrice);
        }
        [Test]
        public void CheckGbpConversion()
        {
            decimal Conversion = Math.Round(UsdPrice * GbpRate, 2);
            Assert.AreEqual(Conversion, GbpPrice);
        }

    }
}
