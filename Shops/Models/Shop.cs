using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        public Shop(string name, uint id, double balance, string address)
        {
            Name = name;
            Id = id;
            Balance = balance;
            Address = address;
            DictOfProducts = new Dictionary<Item, uint>();
            DictForPurchase = new Dictionary<Item, uint>();
        }

        public string Name { get; private set; }
        public uint Id { get; private set; }
        public double Balance { get; private set; }
        public string Address { get; private set; }
        public Dictionary<Item, uint> DictOfProducts { get; set; }
        private Dictionary<Item, uint> DictForPurchase { get; set; }

        public void AddDictForPurchase(Item item, uint amount)
        {
            if (item == null || amount < 1)
            {
                throw new ShopException("Invalid item or amount data");
            }

            DictForPurchase.Add(item, amount);
        }

        public void Purchase()
        {
            double purchaseCost = 0;
            double positionCost = 0;

            foreach (KeyValuePair<Item, uint> pair in DictForPurchase)
            {
                positionCost = pair.Key.Price * pair.Value;
                purchaseCost += positionCost;
            }

            if (purchaseCost > Balance)
            {
                throw new ShopException("Cost of products for purchase more than balance of shop");
            }

            foreach (KeyValuePair<Item, uint> pair in DictForPurchase)
            {
                if (DictOfProducts.ContainsKey(pair.Key))
                {
                    DictOfProducts[pair.Key] += pair.Value;
                }
                else
                {
                    DictOfProducts.Add(pair.Key, pair.Value);
                }
            }

            Balance -= purchaseCost;
            DictForPurchase.Clear();
        }

        public bool IsItemInStock(Item item, uint amount)
        {
            if (DictOfProducts.ContainsKey(item))
            {
                foreach (KeyValuePair<Item, uint> pair in DictOfProducts)
                {
                    if (pair.Key.Equals(item))
                    {
                        if (pair.Value < amount)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                throw new ShopException("There is no such an item in this shop");
            }

            return false;
        }

        public bool IsItemInStockByItemName(Item item, uint amount)
        {
            foreach (Item itemForEach in DictOfProducts.Keys)
            {
                if (itemForEach.Name == item.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCartInStock(Dictionary<Item, uint> cart)
        {
            foreach (KeyValuePair<Item, uint> pair in cart)
            {
                if (DictOfProducts.Keys.Contains(pair.Key))
                {
                    if (DictOfProducts[pair.Key] < pair.Value)
                    {
                        throw new ShopException("Not enogh amount of Item");
                    }
                }
                else
                {
                    throw new ShopException("There is no one of items from the cart in the shop");
                }
            }

            return true;
        }

        public void ShopItemsWriteoff(Dictionary<Item, uint> cart)
        {
            foreach (KeyValuePair<Item, uint> pair in cart)
            {
                foreach (KeyValuePair<Item, uint> position in DictOfProducts)
                {
                    if (pair.Key.Equals(position.Key))
                    {
                        if (pair.Value == position.Value)
                        {
                            DictOfProducts.Remove(position.Key);
                        }
                        else
                        {
                            DictOfProducts[position.Key] -= pair.Value;
                        }
                    }
                }
            }
        }

        public void ShopBalanceIncome(double cost)
        {
            Balance += cost;
        }

        public bool IsItemAvailableInShop(Item item)
        {
            foreach (KeyValuePair<Item, uint> pair in DictOfProducts)
            {
                if (pair.Key.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public double ItemPriceByName(Item item)
        {
            foreach (KeyValuePair<Item, uint> position in DictOfProducts)
            {
                if (position.Key.Name == item.Name) return position.Key.Price;
            }

            throw new ShopException("there is ni item with such a name in this shop");
        }

        public Item GetItemFromShop(Item item)
        {
            foreach (KeyValuePair<Item, uint> pair in DictOfProducts)
            {
                if (item.Id == pair.Key.Id)
                {
                    return pair.Key;
                }
            }

            throw new ShopException("there is no such an item in this shop");
        }
    }
}