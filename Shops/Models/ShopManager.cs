using System;
using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Models
{
    public class ShopManager
    {
        public ShopManager()
        {
            ListOfShops = new List<Shop>();
            ListOfItems = new List<Item>();
        }

        public List<Shop> ListOfShops { get; private set; }
        public List<Item> ListOfItems { get; private set; }
        public uint ShopId { get; private set; } = 0;
        public uint UserId { get; private set; } = 0;
        public uint ItemId { get; private set; } = 0;

        public Shop CreateShop(string name, double balance, string address)
        {
            if (balance < 0 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
            {
                throw new ShopException("Invalid data");
            }

            ShopId++;
            var currentShop = new Shop(name, ShopId, balance, address);
            ListOfShops.Add(currentShop);
            return currentShop;
        }

        public Item CreateItem(string name, double price)
        {
            if (string.IsNullOrEmpty(name) || price < 0)
            {
                throw new ShopException("Invalid data");
            }

            ItemId++;
            var currentItem = new Item(name, ItemId, price);
            ListOfItems.Add(currentItem);
            return currentItem;
        }

        public void AddItemForPurchase(Shop shop, Item item, uint amount)
        {
            shop.AddDictForPurchase(item, amount);
        }

        public void Purchase(Shop shop)
        {
            shop.Purchase();
        }

        public void InitializationOfBuying(User user, Shop shop)
        {
            if (user.Balance < 0 || user == null || shop == null)
            {
                throw new ShopException("Invalid data for initialization of buying");
            }

            user.ChangeShopStatus(shop);
        }

        public void AddItemIntoUserCart(User user, Shop shop, Item item, uint amount)
        {
            if (user == null || !ListOfItems.Contains(item) || !ListOfShops.Contains(shop) || amount < 1)
            {
                throw new ShopException("Invalid data for AddItemIntoUserCart");
            }

            if (shop != user.ShopStatus)
            {
                throw new ShopException("Invalid shop for adding to cart. Initialize shop with InitializationOfBuying or use shop frm ShopStatus");
            }

            if (shop.IsItemInStock(item, amount)) user.AddDictUserCart(item, amount);
            else throw new ShopException("there is not enough item in this shop");
        }

        public void Buy(User user, Shop shop)
        {
            if (shop != user.ShopStatus)
            {
                throw new ShopException("Invalid shop to buy.");
            }

            if (!shop.IsCartInStock(user.DictUserCart))
            {
                throw new ShopException("Cart is not in stock.");
            }

            if (!user.IsBalanceEnoughToBuy(user.CostOfCart()))
            {
                throw new ShopException("Not enough money on balance to buy this cart.");
            }

            user.UserBalanceWriteoff(user.CostOfCart());

            shop.ShopItemsWriteoff(user.DictUserCart);

            shop.ShopBalanceIncome(user.CostOfCart());
        }

        public void ChangeItemPriceInShop(Shop shop, Item item, double price)
        {
            if (!ListOfItems.Contains(item)) throw new ShopException(" there is no such an item");

            if (!ListOfShops.Contains(shop)) throw new ShopException(" there is no such a shop");

            if (shop.IsItemAvailableInShop(item))
            {
                item.ChangeItemPrice(price);
            }
            else
            {
                throw new ShopException("there is no such in item in this shop");
            }
        }

        public Shop LowestPriceForNumberOfItemsInShops(Item item, uint amount)
        {
            if (!ListOfItems.Contains(item)) throw new ShopException(" there is no such an item");

            if (amount < 1) throw new ShopException("minimal number of items is 1");

            double lowestPrice = double.MaxValue;

            Shop shopWithLowestPrice = null;

            foreach (Shop currentShop in ListOfShops)
            {
                if (currentShop.IsItemInStockByItemName(item, amount))
                {
                    double currentPrice = currentShop.ItemPriceByName(item);
                    if (lowestPrice >= currentPrice)
                    {
                        lowestPrice = currentPrice;
                        shopWithLowestPrice = currentShop;
                    }
                }
            }

            if (shopWithLowestPrice != null) return shopWithLowestPrice;
            else throw new ShopException("there is no such a shop with this number of this item");
        }
    }
}