using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CoffeeVendingMachine.v0.Model
{
    public class ManAndMachine
    {
        public Shelf[] Goods { get; set; }

        public int Reciept { get; set; }

        public MoneySlot[] CashIn { get; set; }

        public int AmountInCashIn { get; set; }

        public int AmountInPurse { get; set; }

        public MoneySlot[] Purse { get; set; }
    }
}