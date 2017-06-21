using System;
using CoffeeVendingMachine.v0.Model;
using System.IO;
using System.Threading; 
using Newtonsoft.Json;
using System.Web;

namespace CoffeeVendingMachine.v0.DataStorage
{
    public class ManAndMachineRepository : IManAndMachineRepository
    {
        private const string InitialData = "~/App_Data/InitialData.json";

        public ManAndMachine GetInitialState()
        {
            return this.Deserialize(
                File.ReadAllText(
                    HttpContext.Current.Server.MapPath("~/App_Data/InitialData.json")));
        }

        public string Serialize(ManAndMachine mam)
        {
            return JsonConvert.SerializeObject(mam);
        }

        public ManAndMachine Deserialize(string currentState)
        {
            return JsonConvert.DeserializeObject<ManAndMachine>(currentState);
        }
    }
}
