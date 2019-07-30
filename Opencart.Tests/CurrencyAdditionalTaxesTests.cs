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
    class CurrencyAdditionalTaxesTests
    {
        IWebDriver MyDriver;
        string SubTotal;
        string FlatShippingRate;
        string FixedTestTax;
        string PercentageTestTax;
        string Total;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            MyDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/admin/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.Id("input-username")).SendKeys("admin");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("Lv414_Taqc" + Keys.Enter);

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Localisation')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Geo Zones')]")).Click();

            MyDriver.FindElement(By.CssSelector("a[data-original-title='Add New']")).Click();
            MyDriver.FindElement(By.Id("input-name")).SendKeys("UA Tax Zone");
            MyDriver.FindElement(By.Id("input-description")).SendKeys("Special Eco Taxes");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Add Geo Zone']")).Click();
            SelectElement Country = new SelectElement(MyDriver.FindElement(By.Name("zone_to_geo_zone[0][country_id]")));
            Country.SelectByText("Ukraine");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Save']")).Click();

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Taxes')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Tax Rates')]")).Click();
            MyDriver.FindElement(By.CssSelector("a[data-original-title='Add New']")).Click();
            MyDriver.FindElement(By.Id("input-name")).SendKeys("FixedTestTax");
            MyDriver.FindElement(By.Id("input-rate")).SendKeys("2");
            SelectElement FixedTaxType = new SelectElement(MyDriver.FindElement(By.Id("input-type")));
            FixedTaxType.SelectByText("Fixed Amount");
            SelectElement FixedGeoZone = new SelectElement(MyDriver.FindElement(By.Id("input-geo-zone")));
            FixedGeoZone.SelectByText("UA Tax Zone");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Save']")).Click();

            MyDriver.FindElement(By.CssSelector("a[data-original-title='Add New']")).Click();
            MyDriver.FindElement(By.Id("input-name")).SendKeys("PercentageTestTax");
            MyDriver.FindElement(By.Id("input-rate")).SendKeys("5");
            SelectElement PercentageTaxType = new SelectElement(MyDriver.FindElement(By.Id("input-type")));
            PercentageTaxType.SelectByText("Percentage");
            SelectElement PercentageGeoZone = new SelectElement(MyDriver.FindElement(By.Id("input-geo-zone")));
            PercentageGeoZone.SelectByText("UA Tax Zone");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Save']")).Click();

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Tax Classes')]")).Click();
            MyDriver.FindElement(By.XPath("//td[contains(text(), 'Taxable Goods')]/following-sibling::td/a")).Click();
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Add Rule']")).Click();
            var AllSelects = MyDriver.FindElements(By.CssSelector("#tax-rule select"));
            SelectElement FixedTaxRateId = new SelectElement(MyDriver.FindElement(By.CssSelector($"#tax-rule-row{AllSelects.Count / 2 - 1} select[name = 'tax_rule[{AllSelects.Count / 2 - 1}][tax_rate_id]']")));
            FixedTaxRateId.SelectByText("FixedTestTax");
            SelectElement FixedTaxBased = new SelectElement(MyDriver.FindElement(By.CssSelector($"#tax-rule-row{AllSelects.Count / 2 - 1} select[name = 'tax_rule[{AllSelects.Count / 2 - 1}][based]']")));
            FixedTaxBased.SelectByText("Shipping Address");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Add Rule']")).Click();
            SelectElement PercentageTaxRateId = new SelectElement(MyDriver.FindElement(By.CssSelector($"#tax-rule-row{AllSelects.Count / 2} select[name = 'tax_rule[{AllSelects.Count / 2}][tax_rate_id]']")));
            PercentageTaxRateId.SelectByText("PercentageTestTax");
            SelectElement PercentageTaxBased = new SelectElement(MyDriver.FindElement(By.CssSelector($"#tax-rule-row{AllSelects.Count / 2} select[name = 'tax_rule[{AllSelects.Count / 2}][based]']")));
            PercentageTaxBased.SelectByText("Shipping Address");
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Save']")).Click();

            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.XPath("//span[contains(text(),'My Account')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            MyDriver.FindElement(By.Id("input-email")).SendKeys("johnsmith@gmail.com");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("12121212" + Keys.Enter);
            MyDriver.FindElement(By.XPath("//a[text()='Phones & PDAs']")).Click();
            MyDriver.FindElement(By.CssSelector(".product-layout .fa.fa-shopping-cart")).Click();
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/admin/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.Id("input-username")).SendKeys("admin");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("Lv414_Taqc" + Keys.Enter);

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Localisation')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Taxes')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Tax Classes')]")).Click();
            MyDriver.FindElement(By.XPath("//td[contains(text(), 'Taxable Goods')]/following-sibling::td/a")).Click(); ;
            foreach (var item in MyDriver.FindElements(By.CssSelector("select[name$='[tax_rate_id]']")))
            {
                SelectElement Select = new SelectElement(item);
                string Text = Select.SelectedOption.Text;
                string Name;
                if (Text == "PercentageTestTax" || Text == "FixedTestTax")
                {
                    Name = item.GetAttribute("name");
                    string quiery = "//select[@name='" + Name + "']/parent::td/following-sibling::td/button[@data-original-title='Remove']";
                    MyDriver.FindElement(By.XPath(quiery)).Click();
                }
            }
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Save']")).Click();

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Tax Rates')]")).Click();
            foreach (var item in MyDriver.FindElements(By.XPath("//td[contains(text(),'FixedTestTax')]/preceding-sibling::td/input")))
                item.Click();
            foreach (var item in MyDriver.FindElements(By.XPath("//td[contains(text(),'PercentageTestTax')]/preceding-sibling::td/input")))
                item.Click();
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Delete']")).Click();
            MyDriver.SwitchTo().Alert().Accept();

            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Geo Zones')]")).Click();
            foreach (var item in MyDriver.FindElements(By.XPath("//td[contains(text(),'UA Tax Zone')]/preceding-sibling::td/input")))
                item.Click();
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Delete']")).Click();
            MyDriver.SwitchTo().Alert().Accept();

            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
            MyDriver.FindElement(By.CssSelector("button[data-original-title='Remove']")).Click();

            MyDriver.Quit();
        }

        [TestCase("USD", @"^\$\d+\.\d{2}")]
        [TestCase("EUR", @"\d+\.\d{2}€$")]
        [TestCase("GBP", @"^£\d+\.\d{2}")]
        public void CheckCurrencyFormat(string currency, string pattern)
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            MyDriver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement ShippingCountry = new SelectElement(MyDriver.FindElement(By.Id("input-country")));
            ShippingCountry.SelectByText("Ukraine");
            SelectElement Zone = new SelectElement(MyDriver.FindElement(By.Id("input-zone")));
            Zone.SelectByText("L'vivs'ka Oblast'");
            MyDriver.FindElement(By.Id("input-postcode")).Clear();
            MyDriver.FindElement(By.Id("input-postcode")).SendKeys("123456");
            MyDriver.FindElement(By.Id("button-quote")).Click();
            MyDriver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            MyDriver.FindElement(By.Id("button-shipping")).Click();
            SubTotal = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Sub-Total:')]/parent::td/following-sibling::td")).Text;
            FlatShippingRate = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Flat Shipping Rate:')]/parent::td/following-sibling::td")).Text;
            FixedTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'FixedTestTax:')]/parent::td/following-sibling::td")).Text;
            PercentageTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'PercentageTestTax:')]/parent::td/following-sibling::td")).Text;
            Total = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[text()='Total:']/parent::td/following-sibling::td")).Text;

            StringAssert.IsMatch(pattern, SubTotal);
            Console.WriteLine(SubTotal);
            StringAssert.IsMatch(pattern, Total);
            Console.WriteLine(Total);
            StringAssert.IsMatch(pattern, FixedTestTax);
            Console.WriteLine(FixedTestTax);
            StringAssert.IsMatch(pattern, PercentageTestTax);
            Console.WriteLine(PercentageTestTax);
        }

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        public void CheckCorrectCalculationOfPersentageTax(string currency)
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            MyDriver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement ShippingCountry = new SelectElement(MyDriver.FindElement(By.Id("input-country")));
            ShippingCountry.SelectByText("Ukraine");
            SelectElement Zone = new SelectElement(MyDriver.FindElement(By.Id("input-zone")));
            Zone.SelectByText("L'vivs'ka Oblast'");
            MyDriver.FindElement(By.Id("input-postcode")).Clear();
            MyDriver.FindElement(By.Id("input-postcode")).SendKeys("123456");
            MyDriver.FindElement(By.Id("button-quote")).Click();
            MyDriver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            MyDriver.FindElement(By.Id("button-shipping")).Click();
            SubTotal = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Sub-Total:')]/parent::td/following-sibling::td")).Text;
            FlatShippingRate = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Flat Shipping Rate:')]/parent::td/following-sibling::td")).Text;
            PercentageTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'PercentageTestTax:')]/parent::td/following-sibling::td")).Text;
            Total = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[text()='Total:']/parent::td/following-sibling::td")).Text;

            decimal SubTotalValue = Decimal.Parse(Regex.Match(SubTotal, @"\d+\.\d{2}").Value.Replace(".",","));
            decimal ActualPercentageTestTaxTaxValue = Decimal.Parse(Regex.Match(PercentageTestTax, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal FlatShippingRateValue = Decimal.Parse(Regex.Match(FlatShippingRate, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal ExpectedPercentageTestTaxValue = Math.Round((SubTotalValue + FlatShippingRateValue) * 0.05m, 2);
            Console.WriteLine(ActualPercentageTestTaxTaxValue);
            Console.WriteLine(ExpectedPercentageTestTaxValue);
            Assert.AreEqual(ExpectedPercentageTestTaxValue, ActualPercentageTestTaxTaxValue);
        }
        
        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        public void CheckFixedTaxHasTheSameValueAsInAdminPanel(string currency)
        {
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/admin/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.Id("input-username")).SendKeys("admin");
            MyDriver.FindElement(By.Id("input-password")).SendKeys("Lv414_Taqc" + Keys.Enter);
            MyDriver.FindElement(By.Id("menu-system")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Localisation')]")).Click();
            MyDriver.FindElement(By.XPath("//a[contains(text(),'Currencies')]")).Click();            

            decimal XRate = Decimal.Parse(MyDriver.FindElement(By.XPath($"//td[text()='{currency}']/following-sibling::td[@class='text-right'][count(*)=0]")).Text.Replace('.', ','));
            Console.WriteLine(XRate);

            MyDriver.FindElement(By.CssSelector("#header .nav.pull-right>li>a[href]")).Click();

            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            MyDriver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement ShippingCountry = new SelectElement(MyDriver.FindElement(By.Id("input-country")));
            ShippingCountry.SelectByText("Ukraine");
            SelectElement Zone = new SelectElement(MyDriver.FindElement(By.Id("input-zone")));
            Zone.SelectByText("L'vivs'ka Oblast'");
            MyDriver.FindElement(By.Id("input-postcode")).Clear();
            MyDriver.FindElement(By.Id("input-postcode")).SendKeys("123456");
            MyDriver.FindElement(By.Id("button-quote")).Click();
            MyDriver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            MyDriver.FindElement(By.Id("button-shipping")).Click();

            int Quantity = Int32.Parse(MyDriver.FindElement(By.CssSelector("input[name^='quantity']")).GetAttribute("value"));
            FixedTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'FixedTestTax:')]/parent::td/following-sibling::td")).Text;
            decimal ActualFixedTestTaxValue = Decimal.Parse(Regex.Match(FixedTestTax, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal ExpectedFixedTestTaxValue =Math.Round(2m * XRate * Quantity, 2);
            Console.WriteLine(ActualFixedTestTaxValue);
            Console.WriteLine(ExpectedFixedTestTaxValue);
            Assert.AreEqual(ExpectedFixedTestTaxValue, ActualFixedTestTaxValue);
        }

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        public void CheckCorrectCulculationOfTotal(string currency)
        {
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.FindElement(By.CssSelector("a[title='Shopping Cart']")).Click();
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            MyDriver.FindElement(By.CssSelector("a[href='#collapse-shipping']")).Click();
            SelectElement ShippingCountry = new SelectElement(MyDriver.FindElement(By.Id("input-country")));
            ShippingCountry.SelectByText("Ukraine");
            SelectElement Zone = new SelectElement(MyDriver.FindElement(By.Id("input-zone")));
            Zone.SelectByText("L'vivs'ka Oblast'");
            MyDriver.FindElement(By.Id("input-postcode")).Clear();
            MyDriver.FindElement(By.Id("input-postcode")).SendKeys("123456");
            MyDriver.FindElement(By.Id("button-quote")).Click();
            MyDriver.FindElement(By.CssSelector("#modal-shipping input[value='flat.flat']")).Click();
            MyDriver.FindElement(By.Id("button-shipping")).Click();

            string SubTotal = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Sub-Total:')]/parent::td/following-sibling::td")).Text;
            string FlatShippingRate = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'Flat Shipping Rate:')]/parent::td/following-sibling::td")).Text;
            string PercentageTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'PercentageTestTax:')]/parent::td/following-sibling::td")).Text;
            string Total = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[text()='Total:']/parent::td/following-sibling::td")).Text;
            string FixedTestTax = MyDriver.FindElement(By.XPath("//div[@id='content']//strong[contains(text(),'FixedTestTax:')]/parent::td/following-sibling::td")).Text;

            decimal FixedTestTaxValue = Decimal.Parse(Regex.Match(FixedTestTax, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal SubTotalValue = Decimal.Parse(Regex.Match(SubTotal, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal PercentageTestTaxTaxValue = Decimal.Parse(Regex.Match(PercentageTestTax, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal FlatShippingRateValue = Decimal.Parse(Regex.Match(FlatShippingRate, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal ActualTotalValue = Decimal.Parse(Regex.Match(Total, @"\d+\.\d{2}").Value.Replace(".", ","));
            decimal ExpectedTotalValue = SubTotalValue + FlatShippingRateValue + FixedTestTaxValue + PercentageTestTaxTaxValue;

            Console.WriteLine(ExpectedTotalValue);
            Console.WriteLine(ActualTotalValue);

            Assert.AreEqual(ExpectedTotalValue, ActualTotalValue);
        }
    }
}
