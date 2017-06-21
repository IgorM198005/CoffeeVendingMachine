using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeVendingMachine.v0.Model
{
    public interface IManAndMachineStateStrategy
    {
        bool CanSell(ManAndMachine mam, int slotIndex);
        bool CanRevert(ManAndMachine mam);
        void Sell(ManAndMachine mam, int slotIndex);
        void RevertMoneyToPurse(ManAndMachine mam);
        void PushMoneyToMachine(ManAndMachine mam, int slotIndex);        
    }
}
