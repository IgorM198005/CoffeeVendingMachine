using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeVendingMachine.v0.Model
{
    public class CoinSelectionStrategy : ICoinSelectionStrategy
    {
        public void CashBack(ManAndMachine mam, int amount)
        {
            int balance = amount;

            for (int i = 0; i < mam.CashIn.Length; i++)
            {
                int count = balance / mam.CashIn[i].Nominal;

                if (count > mam.CashIn[i].Count)
                {
                    count = mam.CashIn[i].Count;
                }

                mam.CashIn[i].Count -= count;

                mam.Purse[i].Count += count;

                balance -= count * mam.CashIn[i].Nominal;
            }
        }
    }
}
