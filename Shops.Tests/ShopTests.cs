using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Shops.Models;
using NUnit.Framework;
using Shops.Tools;


namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _shopManager;
        private Item _item1;
        private Item _item2;
        private Item _item3;
        private Item _item4;

        [SetUp]
        public void SetUp()
        { 
            _shopManager = new ShopManager();
            _item1 = _shopManager.CreateItem("yeezy", 100); 
            _item2 = _shopManager.CreateItem("nike", 80);
            _item3 = _shopManager.CreateItem("yeezy", 20);
            _item4 = _shopManager.CreateItem("yeezy", 70);
        }

        [Test]
        public void PurchaseItemToShop_Success()
        {
            double testBalance = 10000;
            Shop shop = _shopManager.CreateShop("Bespalov", testBalance, "SPB");

            _shopManager.AddItemForPurchase(shop, _item1, 10);
            _shopManager.AddItemForPurchase(shop, _item2, 5);

            _shopManager.Purchase(shop);

            Assert.True(shop.DictOfProducts.ContainsKey(_item1));
            Assert.True(shop.DictOfProducts.ContainsKey(_item2));
            testBalance -= _item1.Price * 10 + _item2.Price * 5;
            Assert.AreEqual(testBalance, shop.Balance);
        }

        [Test]
        public void PurchaseItemsToShopAndUserBuyItem_Success()
        {
            double testShopBalance = 10000;
            double testUserBalance = 1000;
            uint amountYeezy = 10;
            Shop shop = _shopManager.CreateShop("Bespalov", testShopBalance, "SPB");

            _shopManager.AddItemForPurchase(shop, _item1, amountYeezy);
            _shopManager.AddItemForPurchase(shop, _item2, 5);

            _shopManager.Purchase(shop);
            
            testShopBalance-= _item1.Price * 10 + _item2.Price * 5;
            
            var user = new User("Fred", 1, testUserBalance);
            _shopManager.InitializationOfBuying(user, shop);
            _shopManager.AddItemIntoUserCart(user, shop, _item1, 1);
            _shopManager.Buy(user, shop);

            Assert.AreEqual(testShopBalance + _item1.Price, shop.Balance);
            Assert.AreEqual(testUserBalance - _item1.Price, user.Balance);
            Assert.AreEqual(shop.DictOfProducts[_item1], amountYeezy - 1);
        }

        [Test]
        public void ChangePriceOfItem_Success()
        {
            double testBalance = 10000;
            double newPrice = 90;
            Shop shop = _shopManager.CreateShop("Bespalov", testBalance, "SPB");
            _shopManager.AddItemForPurchase(shop, _item1, 10);
            _shopManager.AddItemForPurchase(shop, _item2, 5);

            _shopManager.Purchase(shop);

            _shopManager.ChangeItemPriceInShop(shop, _item2, newPrice);
            Assert.AreEqual(newPrice, shop.GetItemFromShop(_item2).Price);
        }

        [Test]
        public void FindShopWithLowestPriceOfNumberOfItem_Success()
        {
            double testBalance = 10000;
            Shop shop1 = _shopManager.CreateShop("Bespalov", testBalance, "SPB");
            _shopManager.AddItemForPurchase(shop1, _item1, 10);
            _shopManager.AddItemForPurchase(shop1, _item2, 5);

            _shopManager.Purchase(shop1);
            
            Shop shop2 = _shopManager.CreateShop("Farfetch", testBalance, "SPB");
            _shopManager.AddItemForPurchase(shop2, _item3, 10);

            _shopManager.Purchase(shop2);
            
            Shop shop3 = _shopManager.CreateShop("KM20", testBalance, "SPB");
            _shopManager.AddItemForPurchase(shop3, _item4, 10);

            _shopManager.Purchase(shop3);

            Shop shopWithLowestPrice = _shopManager.LowestPriceForNumberOfItemsInShops(_item1, 5);
            Assert.AreEqual(shopWithLowestPrice.Name, shop2.Name);
        }

        [Test]
        public void UserDontHaveEnoughMoneyToBuyItem_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                double testUserBalance = 10;
                double testShopBalance = 10000;
                uint amountYeezy = 10;
                
                var user = new User("Fred", 1, testUserBalance);
                
                Shop shop = _shopManager.CreateShop("Bespalov", testShopBalance, "SPB");

                _shopManager.AddItemForPurchase(shop, _item1, amountYeezy);
                _shopManager.Purchase(shop);
                
                _shopManager.InitializationOfBuying(user, shop);
                _shopManager.AddItemIntoUserCart(user, shop, _item1, 1);
                _shopManager.Buy(user, shop);

            });
        }

        [Test]
        public void UserCantBuyItemBecauseThereIsNoSuchAnItemInThisShop_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                double testUserBalance = 10;
                double testShopBalance = 10000;
                uint amountYeezy = 10;
                
                var user = new User("Fred", 1, testUserBalance);
                
                Shop shop = _shopManager.CreateShop("Bespalov", testShopBalance, "SPB");

                _shopManager.AddItemForPurchase(shop, _item1, amountYeezy);
                _shopManager.Purchase(shop);
                
                _shopManager.InitializationOfBuying(user, shop);
                _shopManager.AddItemIntoUserCart(user, shop, _item2, 1);
                _shopManager.Buy(user, shop);
            });
        }

    }
}