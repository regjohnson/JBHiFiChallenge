using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace JBHiFiChallengeWeb.Tests.UITests
{
    public class UITestsShould
    {
        string url = "https://localhost:44357/";

        [Fact]
        public void LoadApplicationPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(url);
                string pageTitle = driver.Title;

                Assert.Equal("JB HiFi Challenge", pageTitle);
            }
        }

        [Fact]
        public void PerformSearch()
        {
            var resultText = GetSearchResult("Melbourne", "AUS", "Key2");
            Assert.NotEqual(string.Empty, resultText ?? string.Empty);
        }

        [Fact]
        public void PerformSearchWithoutCity()
        {
            var resultText = GetSearchResult(string.Empty, "AUS", "Key3");
            Assert.Equal("City name was not provided", resultText);
        }

        [Fact]
        public void PerformSearchWithoutCountry()
        {
            var resultText = GetSearchResult("Melbourne", string.Empty, "Key3");
            Assert.Equal("Country name was not provided", resultText);
        }

        [Fact]
        public void PerformSearchWithoutKeyName()
        {
            var resultText = GetSearchResult("Melbourne", "AUS", string.Empty);
            Assert.Equal("Key name was not sent in header", resultText);
        }

        [Fact]
        public void PerformSearchWithInvaildKeyName()
        {
            var resultText = GetSearchResult("Melbourne", "AUS", "Key123");
            Assert.Equal("Invalid Key name was sent", resultText);
        }

        private string GetSearchResult(string cityName, string countryName, string keyName)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(url);
                IWebElement cityElement = driver.FindElement(By.Id("txtCity"));
                cityElement.SendKeys(cityName);

                IWebElement countryElement = driver.FindElement(By.Id("txtCountry"));
                countryElement.SendKeys(countryName);

                IWebElement keyNameElement = driver.FindElement(By.Id("txtKeyName"));
                keyNameElement.Clear();
                keyNameElement.SendKeys(keyName);

                IWebElement btnSearch = driver.FindElement(By.Id("btnSearch"));
                btnSearch.Click();

                //To wait for element visible
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
                var resultElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("searchResult")));

                string text = resultElement.Text?.Trim('"');
                return text;
            }
        }
    }
}
