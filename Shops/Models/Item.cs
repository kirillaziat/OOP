using System;
using Shops.Tools;

namespace Shops.Models
{
    public class Item : IEquatable<Item>
    {
        public Item(string name, uint id, double price)
        {
            Name = name;
            Id = id;
            Price = price;
        }

        public string Name { get; private set; }
        public uint Id { get; private set; }
        public double Price { get; private set; }

        public void ChangeItemPrice(double newPrice)
        {
            if (newPrice < 0)
            {
                throw new ShopException("price can not be less than 0");
            }

            Price = newPrice;
        }

        public bool Equals(Item other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Id == other.Id && Price.Equals(other.Price);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id, Price);
        }
    }
}