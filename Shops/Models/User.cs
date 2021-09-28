using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Models
{
    public class User
    {
        public User(string name, uint id, double balance)
        {
            Name = name;
            Id = id;
            Balance = balance;
            DictUserCart = new Dictionary<Item, uint>();
            ShopStatus = null;
        }

        public string Name { get; private set; }
        public uint Id { get; private set; }
        public double Balance { get; private set; }
        public Shop ShopStatus { get; private set; }
        public Dictionary<Item, uint> DictUserCart { get; private set; }

        public double CostOfCart()
        {
            double costOfCart = 0;

            foreach (KeyValuePair<Item, uint> pair in DictUserCart)
            {
                costOfCart += pair.Key.Price * pair.Value;
            }

            return costOfCart;
        }

        public void AddDictUserCart(Item item, uint amount)
        {
            if (item.Price * amount > Balance)
            {
                throw new ShopException("Not enough money on user`s balance");
            }

            DictUserCart.Add(item, amount);
        }

        public void ChangeShopStatus(Shop shop)
        {
            ShopStatus = shop;
        }

        public bool IsBalanceEnoughToBuy(double costOfCart)
        {
            if (costOfCart > Balance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UserBalanceWriteoff(double cost)
        {
            if (cost > Balance)
            {
                throw new ShopException("Not enough money on user`s balance");
            }

            Balance -= cost;
        }
    }
}