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
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/admin/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.Id("input-username")).SendKeys("admin");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("Lv414_Taqc" + Keys.Enter);
            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Localisation')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Currencies')]")).Click();

            EurRate = Decimal.Parse(MyDriver.FindElement(By.XPath("//td[text()='EUR']/following-sibling::td[@class='text-right'][count(*)=0]")).Text.Replace('.', ','));
            GbpRate = Decimal.Parse(MyDriver.FindElement(By.XPath("//td[text()='GBP']/following-sibling::td[@class='text-right'][count(*)=0]")).Text.Replace('.', ','));

            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.CssSelector(".owl-wrapper-outer")).Click();
            UsdPrice = Decimal.Parse(MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text.Substring(1).Replace('.', ','));
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name("EUR")).Click();
            EurPrice = Decimal.Parse(MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text.Substring(0, 6).Replace('.', ','));
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name("GBP")).Click();
            GbpPrice = Decimal.Parse(MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text.Substring(1).Replace('.', ','));
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
