using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeVendingMachine.v0.Model
{
    public class ManAndMachineStateStrategy : IManAndMachineStateStrategy
    {
        private ICoinSelectionStrategy mCoinSelectionStrategy;

        public ManAndMachineStateStrategy(ICoinSelectionStrategy coinSelectionStrategy)
        {
            this.mCoinSelectionStrategy = coinSelectionStrategy;
        }

        public bool CanSell(ManAndMachine mam, int slotIndex)
        {
            return mam.Reciept >= mam.Goods[slotIndex].Price;
        }
        
        public bool CanRevert(ManAndMachine mam)
        {
            return mam.Reciept > 0;
        }

        public void Sell(ManAndMachine mam, int slotIndex)
        {
            mam.Goods[slotIndex].Count--;

            int charge = mam.Reciept - mam.Goods[slotIndex].Price;

            this.mCoinSelectionStrategy.CashBack(mam, charge);

            this.CashBackAmounts(mam, charge);
        }

        private void CashBackAmounts(ManAndMachine mam, int amount)
        {
            mam.AmountInCashIn -= amount;

            mam.AmountInPurse += amount;

            mam.Reciept = 0;
        }

        public void RevertMoneyToPurse(ManAndMachine mam)
        {
            int reciept = mam.Reciept;

            this.mCoinSelectionStrategy.CashBack(mam, reciept);

            this.CashBackAmounts(mam, reciept); 
        }

        public void PushMoneyToMachine(ManAndMachine mam, int slotIndex)
        {
            mam.Purse[slotIndex].Count--;

            int nominal = mam.Purse[slotIndex].Nominal;

            mam.AmountInPurse -= nominal;

            mam.CashIn[slotIndex].Count++;

            mam.Reciept += nominal;

            mam.AmountInCashIn += nominal;
        }

    }
}
