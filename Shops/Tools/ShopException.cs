using System;
using System.Runtime.Serialization;

namespace Shops.Tools
{
    public class ShopException : Exception
    {
         public ShopException()
         {
         }

         public ShopException(string message)
             : base(message)
         {
         }
    }
 }