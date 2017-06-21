using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeVendingMachine.v0.Model
{
    public class Shelf
    {
        public string Key { get; set; }
        public string Goods { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}