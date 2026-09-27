using OpenQA.Selenium;
using Selenium_CSharp_Practice.Utils;
using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.DataTypes;

namespace Selenium_CSharp_Practice.Pages
{
    public class InventoryPage : BasePage
    {
        private static readonly By PageTitleLoc = By.XPath("//span[text()='Products']");
        private static readonly By ItemCards = By.ClassName("inventory_item");
        private static readonly By ItemPrices = By.ClassName("inventory_item_price");
        private static readonly By ItemNames = By.ClassName("inventory_item_name");
        private static readonly By ItemDescriptions = By.ClassName("inventory_item_desc");
        private static readonly By ItemImages = By.CssSelector(".inventory_item_img img");
        private static readonly By AddToCartButton = By.TagName("button");
        private static readonly By SortDropdown = By.ClassName("product_sort_container");
        private static readonly By CartLink = By.ClassName("shopping_cart_link");
        private static readonly By CartBadge = By.XPath("//a[@class='shopping_cart_link']//span");
        private static readonly By AllLinks = By.TagName("a");



        public InventoryPage(IWebDriver driver, WaitHelper waits) : base(driver, waits)
        {
        }

        public string GetPageTitle()
        {
            return GetText(PageTitleLoc);
        }

        public List<string> GetItemNames() => FindElements(ItemNames).Select(e => e.Text).ToList();

        public int GetInventoryItemCount() => FindElements(ItemCards).Count;

        public List<InventoryItemCardType> GetProductDetails()
        {
            return FindElements(ItemCards).Select(card => new InventoryItemCardType
            {
                Name = card.FindElement(ItemNames).Text,
                Description = card.FindElement(ItemDescriptions).Text,
                PriceText = card.FindElement(ItemPrices).Text,
                ImageSrc = card.FindElement(ItemImages).GetAttribute("src") ?? string.Empty
            }).ToList();
        }

        // Get All Links from page
        public IReadOnlyCollection<IWebElement> GetAllLinks() => FindElements(AllLinks);

        // Select Product 
        public IWebElement SelectProductCard(string productName)
        {
            return waits.Until(_ =>
        FindElements(ItemCards)
            .FirstOrDefault(card => card.FindElement(ItemNames).Text == productName));

        }

        // Add Product to cart 
        public void AddProductToCart(string productName)
        {
            SelectProductCard(productName).FindElement(AddToCartButton).Click();
            waits.Until(_ => AddToCartButtonText(productName) == "Remove");
        }

        // Add To Cart Button Text
        public string AddToCartButtonText(string productName)
        {
            return SelectProductCard(productName).FindElement(AddToCartButton).Text;
        }

        // Cart Badge Count
        public string GetCartBadgeCount() => FindElement(CartBadge).Text;


    }
}
