using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Support.UI;
using System.Data.SqlClient;

namespace Opencart.Tests
{
    public static class ServiceMethodsSet
    {
        public static void UserLogIn(IWebDriver driver, string userName, string password)
        {
            
            driver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.XPath("//span[contains(text(),'My Account')]")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            driver.FindElement(By.Id("input-email")).SendKeys(userName);
            driver.FindElement(By.Id("input-password")).SendKeys(password + Keys.Enter);
        }

        public static void AdminLogIn(IWebDriver driver, string adminName, string password)
        {
            driver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/admin/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("input-username")).SendKeys("admin");
            driver.FindElement(By.Id("input-password")).SendKeys("Lv414_Taqc" + Keys.Enter);
        }

        public static void AdminLogOut(IWebDriver driver)
        {
            driver.FindElement(By.CssSelector("#header .nav.pull-right>li>a[href]")).Click();
        }

        public static void ChangeCurrency(IWebDriver driver, string currency)
        {
            driver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            driver.FindElement(By.Name(currency)).Click();
        }

        public static void ChooseShippingDetails(IWebDriver driver, string country, string state, string postCode)
        {
            driver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement ShippingCountry = new SelectElement(driver.FindElement(By.Id("input-country")));
            ShippingCountry.SelectByText(country);
            SelectElement Zone = new SelectElement(driver.FindElement(By.Id("input-zone")));
            Zone.SelectByText(state);
            driver.FindElement(By.Id("input-postcode")).Clear();
            driver.FindElement(By.Id("input-postcode")).SendKeys(postCode);
            driver.FindElement(By.Id("button-quote")).Click();
            driver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            driver.FindElement(By.Id("button-shipping")).Click();
        }

        public static void AddProductToShoppingCart(IWebDriver driver)
        {
            driver.FindElement(By.XPath("//a[text()='Phones & PDAs']")).Click();
            driver.FindElement(By.CssSelector(".product-layout .fa.fa-shopping-cart")).Click();
            driver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
        }

        public static decimal GetPriceValue(IWebDriver driver)
        {
            string Price = Regex.Match(driver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text, @"-?\d+\.\d{2}").Value.Replace('.', ',');
            decimal Value = Decimal.Parse(Price);
            return Value;
        }

        public static string GetPriceWithCurrencySymbol(IWebDriver driver)
        {
            string Price = driver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0].Text.Replace('.', ',');

            return Price;
        }


    }
}
