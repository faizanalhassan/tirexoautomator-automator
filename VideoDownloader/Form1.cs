using System;
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
    
        static string downloadlink;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var movieName = searchBox.Text;
            if (String.IsNullOrEmpty(movieName))
            {
                MessageBox.Show("Please Enter Movie Name you want to Search");
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
                int i = 1;
                foreach (var rowElement in cd.FindElements(By.XPath(xpath)))
                { 
                    var provider = rowElement.FindElement(By.XPath("./td[1]"));
                    var size = rowElement.FindElement(By.XPath("./td[3]"));
                    providersComboBox.Items.Add(new { Value = i, Text = $"{provider.Text} | {size.Text}" });
                    i++;
                }
                providersComboBox.DroppedDown = true;
                renameTxtBox.Text = searchBox.Text;

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
            this.ActiveControl = searchBox;

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
            var download_dir = $@"{BASE_DIR}\Download";
            if (!Directory.Exists(download_dir))
            {
                DirectoryInfo download_folder = Directory.CreateDirectory(download_dir);
            }
            chromeOptions.AddUserProfilePreference("download.default_directory", download_dir);
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
            this.Hide();
            cd.Close();
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
            while(cd.FindElements(By.XPath("//iframe[contains(@src, 'hcaptcha.com')]")).Count > 0)
            {
                var prevPosition = cd.Manage().Window.Position;
                cd.Manage().Window.Position = new Point(0, 0);
                MessageBox.Show("Please solve captcha and press ok.");
                cd.Manage().Window.Position = prevPosition;
                Thread.Sleep(1000);
            }
            cd.Manage().Timeouts().ImplicitWait = prevTimespan;
        }
        
        private void downloadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string item = hostsComboBox.SelectedItem.ToString().ToLower();

                if (String.IsNullOrEmpty(item))
                    return;
                if (String.IsNullOrEmpty(renameTxtBox.Text))
                {
                    MessageBox.Show("Please Enter Name");
                    return;
                }
                var provider = (providersComboBox.SelectedItem as dynamic);
                if (provider == null)
                {
                    MessageBox.Show("Select valid provider");
                    return;
                }
                if (!cd.Url.Contains("real-debrid.com"))
                {
                    cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    var xpath = $"//table[contains(@class, 'downloadsortsonlink')and thead[tr[th[1][translate(.,'{item.ToUpper()}','{item}')='{item}']]]]/tbody/tr[{provider.Value}]/td[1]/a[.!='Streaming ']";
                    var url = cd.FindElement(By.XPath(xpath)).GetAttribute("href");
                    LoadPage(url);
                    cd.FindElement(By.XPath("//input[@value='Continuer pour voir le lien']")).Click();
                    downloadlink = cd.FindElement(By.XPath("//div[@class='alert']/a")).GetAttribute("href");
                    LoadPage("https://real-debrid.com/");
                }
                cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                while (cd.FindElements(By.XPath("//a[contains(@href, '/login.php')]")).Count > 0)
                {
                    var prevPosition = cd.Manage().Window.Position;
                    cd.Manage().Window.Position = new Point(0, 0);
                    MessageBox.Show("Please sign in to real-debrid and press ok.");
                    cd.Manage().Window.Position = prevPosition;
                    Thread.Sleep(1000);
                }
                cd.FindElement(By.CssSelector("#links")).SendKeys(downloadlink);
                ((IJavaScriptExecutor)cd).ExecuteScript("document.querySelector('#sub_links').click();");
                Thread.Sleep(3000); // TODO: is this needed ?
                ((IJavaScriptExecutor)cd).ExecuteScript(
                    "let a = document.evaluate(\"//div[@class='link-generated']/a[not(contains(., 'VISIONNER'))]\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null ).singleNodeValue;" +
                    @"let ext = /(\.\w{3}) \([\w\.]+\)$/.exec(a.innerText)[1];" +
                    "a.download = arguments[0] + ext;" +
                    "a.click();", renameTxtBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex}");
            }
            
        }
    }
}
