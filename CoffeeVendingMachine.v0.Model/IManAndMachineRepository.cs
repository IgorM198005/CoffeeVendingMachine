using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeVendingMachine.v0.Model
{
    public interface IManAndMachineRepository
    {
        ManAndMachine GetInitialState();
        string Serialize(ManAndMachine mam);
        ManAndMachine Deserialize(string currentState);
    }
}
