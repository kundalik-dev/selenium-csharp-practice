using OpenQA.Selenium;
using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.DataTypes;
using Selenium_CSharp_Practice.Pages;
using Selenium_CSharp_Practice.Utils;

namespace Selenium_CSharp_Practice.Tests
{
    [TestFixture]
    public class InventoryTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private InventoryPage _inventoryPage = null!;


        [SetUp]
        public void LoginAsStandardUser()
        {
            _loginPage = new LoginPage(driver, waits);
            _inventoryPage = _loginPage.LoginAndNavigate("standard_user", "secret_sauce");
        }

        [TestCase("Sauce Labs Backpack")]
        public void Should_LoginAndDisplay_FirstProductName_As_SauceLabsBackpack(string expectedName)
        {
            var itemsList = _inventoryPage.GetItemNames();
            Assert.That(itemsList[0], Is.EqualTo(expectedName));
        }

        [TestCase(6)]
        public void Should_Display_6_InventoryItems(int count)
        {
            Assert.That(count, Is.EqualTo(_inventoryPage.GetInventoryItemCount()));
        }

        [TestCase("Sauce Labs Backpack")]
        public void Should_Display_AllProductDetails(string productName)
        {
            var backpack = _inventoryPage.GetProductDetails().Single(c => c.Name == productName);

            Assert.Multiple((Action)(() =>
            {
                Assert.That(backpack.PriceText, Is.EqualTo("$29.99"));
                Assert.That(backpack.Description, Does.Contain("carry.allTheThings()"));
                Assert.That(backpack.ImageSrc, Is.Not.Empty);
            }));
        }


        [Ignore("Feature not ready")]
        [Test]
        public void Should_NavigateSuccessfully_When_Clicking_AllLinks()
        {

        }

        [Test]
        public void Should_FindBrokenLinks()
        {
            var links = _inventoryPage.GetAllLinks();
            List<string> brokenLinks = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                foreach (var link in links)
                {
                    string href = link.GetAttribute("href");
                    if (string.IsNullOrEmpty(href)) continue;

                    try
                    {
                        var response = client.GetAsync(href).Result; // synchronous for simplicity
                        if (!response.IsSuccessStatusCode) // status >= 400
                        {
                            brokenLinks.Add($"{href} - Status: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        brokenLinks.Add($"{href} - Exception: {ex.Message}");
                    }
                }

            }

            Assert.That(brokenLinks.Count, Is.EqualTo(0), "Broken links found:\n" + string.Join("\n", brokenLinks));
        }

        [Test]
        public void Should_FindBrokenLinks_MyWay()
        {
            var links = _inventoryPage.GetAllLinks();

            List<string> brokenLinks = new List<string>();

            HttpClient client = new HttpClient();

            foreach (IWebElement link in links)
            {
                string href = link.GetAttribute("href");

                if (string.IsNullOrEmpty(href)) continue;

                try
                {
                    var response = client.GetAsync(href).Result;
                    if (!response.IsSuccessStatusCode) // status >= 400
                    {
                        brokenLinks.Add($"{href} - Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    brokenLinks.Add($"{href} - Exception: {ex.Message}");
                }
            }

            Assert.That(brokenLinks.Count, Is.EqualTo(0), "Broken links found:\n" + string.Join("\n", brokenLinks));

        }

        [Ignore("Feature not ready")]
        [Test]
        public void Should_SortProducts_ByPrice_Ascending()
        {
            //var links = _inventoryPage;

        }

        [TestCase("Sauce Labs Backpack", "1")]
        public void Should_AddOneProductToCart(string productName, string expectedProductCount)
        {
            _inventoryPage.AddProductToCart(productName);
            string actualCartItemCount = _inventoryPage.GetCartBadgeCount();

            Assert.That(actualCartItemCount, Is.EqualTo(expectedProductCount));

        }

        [TestCase("Sauce Labs Backpack", "Sauce Labs Fleece Jacket", "2")]
        [Test]
        public void Should_AddTwoProductsToCart(string productName1, string productName2, string expectedProductCount)
        {
            _inventoryPage.AddProductToCart(productName1);
            _inventoryPage.AddProductToCart(productName2);
            string actualCartItemCount = _inventoryPage.GetCartBadgeCount();

            Assert.That(actualCartItemCount, Is.EqualTo(expectedProductCount));
        }

        [TestCase("Sauce Labs Backpack", "Remove")]
        public void Should_ChangeAddToCart_ToRemove(string productName, string expectedButtonText)
        {
            _inventoryPage.AddProductToCart(productName);
            string buttonText = _inventoryPage.AddToCartButtonText(productName);

            Assert.That(buttonText, Is.EqualTo(expectedButtonText));
        }

        [Ignore("Feature not ready")]
        [Test]
        public void Should_SortProducts_ByName_Ascending()
        {

        }

        [Ignore("Feature not ready")]
        [Test]
        public void Should_NavigateToCartPage_OnClickingCartIcon()
        {

        }

        [TestCaseSource(nameof(InventoryProducts))]
        public void Should_Display_ProductDetails_FromTestData(InventoryProductTestData expectedProduct)
        {
            var actualProduct = _inventoryPage
                                  .GetProductDetails()
                                  .Single(product => product.Name == expectedProduct.ProductName);

            Assert.Multiple((Action)(() =>
            {
                Assert.That(actualProduct.PriceText,
                    Is.EqualTo(expectedProduct.ProductPrice));

                Assert.That(actualProduct.Description,
                    Is.EqualTo(expectedProduct.ProductDescription));
            }));
        }

        [Test]
        public void Should_Display_AllProductDetails_FromJsonTestData()
        {
            var expectedProducts = JsonDataProvider
                .GetTestData<InventoryPageTestData>(
                    Path.Combine("TestData", "InventoryPageTestData.json"))
                .Products;

            var actualProducts = _inventoryPage.GetProductDetails();

            Assert.Multiple((Action)(() =>
            {
                Assert.That(actualProducts.Count,
                    Is.EqualTo(expectedProducts.Count),
                    "The UI product count does not match the JSON test data.");

                foreach (var expectedProduct in expectedProducts)
                {
                    var actualProduct = actualProducts.SingleOrDefault(
                        product => product.Name == expectedProduct.ProductName);

                    Assert.That(actualProduct, Is.Not.Null,
                        $"Product '{expectedProduct.ProductName}' was not found in the UI.");

                    if (actualProduct is not null)
                    {
                        Assert.That(actualProduct.PriceText,
                            Is.EqualTo(expectedProduct.ProductPrice),
                            $"Incorrect price for '{expectedProduct.ProductName}'.");
                        Assert.That(actualProduct.Description,
                            Is.EqualTo(expectedProduct.ProductDescription),
                            $"Incorrect description for '{expectedProduct.ProductName}'.");
                    }
                }
            }));
        }

        private static IEnumerable<TestCaseData> InventoryProducts()
        {
            string fileName = "InventoryPageTestData.json";
            string folderName = "TestData";

            var data = JsonDataProvider.GetTestData<InventoryPageTestData>(
                Path.Combine(folderName, fileName));

            foreach (var product in data.Products)
            {
                yield return new TestCaseData(product)
                    .SetName($"Should_Display_ProductDetails_FromTestData_{product.ProductId}");
            }
        }


    }
}
