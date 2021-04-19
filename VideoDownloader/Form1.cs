﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace VideoDownloader
{
    public partial class Form1 : Form
    {
        IWebDriver cd;
        readonly string BASE_DIR = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var movieName = searchBox.Text;
            if (String.IsNullOrEmpty(movieName))
            {
                MessageBox.Show("Please End Movie Name you want to Search");
                return;
            }
            
            try
            {
                LoadMovieQualities(movieName);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Unexpected Error: {ex}");
                return;
            }

        }

        private void MovieResCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cd.FindElement(By.CssSelector("button.dropdown-toggle")).Click();
                var xpath = $"//ul[contains(@class, 'inner show')]/li/a[.='{movieResCombo.SelectedItem}']";
                cd.FindElement(By.XPath(xpath)).Click();
                
                Thread.Sleep(500);
                (new WebDriverWait(cd, TimeSpan.FromSeconds(10))).Until((d)=>d.FindElements(By.XPath("//div[contains(@class, 'spinner-border') and @style='display: none;']")));
                hostsComboBox.Items.Clear();
                foreach(var element in cd.FindElements(By.XPath("//table[contains(@class, 'downloadsortsonlink')]//tr/th[1]")))
                {
                    hostsComboBox.Items.Add(element.Text);
                }
                hostsComboBox.DroppedDown = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex}");
                return;
            }
        }
        private void HostsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = hostsComboBox.SelectedItem.ToString().ToLower();
            if (String.IsNullOrEmpty(item))
                return;
            try
            {
                var xpath = $"//table[contains(@class, 'downloadsortsonlink')and thead[tr[th[1][translate(.,'{item.ToUpper()}','{item}')='{item}']]]]/tbody/tr";
                providersComboBox.Items.Clear();
                foreach (var rowElement in cd.FindElements(By.XPath(xpath)))
                {
                    var provider = rowElement.FindElement(By.XPath("./td[1]"));
                    var size = rowElement.FindElement(By.XPath("./td[3]"));
                    providersComboBox.Items.Add(new { Value = provider.Text, Text = $"{provider.Text} | {size.Text}" });

                }
                providersComboBox.DroppedDown = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex}");
                return;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RunChromeDriver();
            providersComboBox.DisplayMember = "Text";
            providersComboBox.ValueMember = "Value";

        }
        private void RunChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument($@"--user-data-dir={BASE_DIR}\cd");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-setuid-sandbox");
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--ignore-certificate-errors-spki-list");
            chromeOptions.AddArgument("--log-level=3");
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            cd = new ChromeDriver(chromeDriverService, chromeOptions);
            //TODO: cd.Manage().Window.Position = new Point(-10000, 0); 
            ((IJavaScriptExecutor)cd).ExecuteScript("delete navigator.__proto__.webdriver");
            cd.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(40);
        }
        private bool LoadMovieQualities(string movieName)
        {
            cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            LoadPage("https://www2.tirexo.ai/");
            cd.FindElement(By.CssSelector(".search-form-control")).SendKeys(movieName + OpenQA.Selenium.Keys.Return);
            var results = cd.FindElements(By.CssSelector(".mov-t.nowrap"));
            IWebElement resultElement;
            if (results.Count < 0 || (resultElement = results[0]).GetAttribute("title").ToLower() != movieName.ToLower())
            {
                MessageBox.Show("Sorry! Couldn't find the desired movie");
                return false;
            }
            LoadPage(resultElement.GetAttribute("href"));
            cd.FindElement(By.CssSelector("button.dropdown-toggle")).Click();
            movieResCombo.Items.Clear();
            foreach (var element in cd.FindElements(By.CssSelector("ul.inner.show li span")))
            {
                movieResCombo.Items.Add(element.Text);
            }
            cd.FindElement(By.CssSelector("button.dropdown-toggle")).Click();
            cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            movieResCombo.DroppedDown = true;
            return true;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cd.Close();
            this.Hide();
            cd.Quit();
        }
        private void LoadPage(string url)
        {
            var prevTimespan = cd.Manage().Timeouts().ImplicitWait;
            cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            cd.Navigate().GoToUrl(url);

            int tryCount = 0;
            var cloudfareElementsCount = cd.FindElements(By.XPath("//*[@id='cf-content']/h1[.='Checking your browser before accessing tirexo.ai.']")).Count;
            while (cloudfareElementsCount > 0)
            {
                if(tryCount > 3)
                {
                    throw new Exception("Could not load page bcz of cloudfare");
                }
                tryCount++;
                Thread.Sleep(5000);
            }
            cd.Manage().Timeouts().ImplicitWait = prevTimespan;
        }

        
    }
}