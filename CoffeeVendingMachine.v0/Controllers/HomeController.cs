using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoffeeVendingMachine.v0.Model;
using Newtonsoft.Json;

namespace CoffeeVendingMachine.v0.Controllers
{
    public class HomeController : Controller
    {
        private IManAndMachineRepository mRepo;

        private IManAndMachineStateStrategy mStateStrategy;

        public HomeController(IManAndMachineRepository repo, IManAndMachineStateStrategy stateStrategy)
        {
            this.mRepo = repo;

            this.mStateStrategy = stateStrategy; 
        }

        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];

            ManAndMachine mam = TempData["CurrentModel"] as ManAndMachine;

            if (mam == null)
            {
                mam = this.mRepo.GetInitialState(); 
            }

            ViewBag.Serialized = this.mRepo.Serialize(mam); 

            return View(mam);
        }

        [HttpPost]
        public ActionResult RevertMoney(string currentState)
        {
            ManAndMachine mam = this.mRepo.Deserialize(currentState); 

            if (this.mStateStrategy.CanRevert(mam))
            {
                this.mStateStrategy.RevertMoneyToPurse(mam);                
            }
            else
            {
                TempData["Message"] = "Возвращать нечего !";
            }

            TempData["CurrentModel"] = mam;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PushMoney(string currentState, int slotIndex)
        {
            ManAndMachine mam = this.mRepo.Deserialize(currentState);

            this.mStateStrategy.PushMoneyToMachine(mam, slotIndex);

            TempData["CurrentModel"] = mam;

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public ActionResult Sell(string currentState, int slotIndex)
        {
            ManAndMachine mam = this.mRepo.Deserialize(currentState);

            if (this.mStateStrategy.CanSell(mam, slotIndex))
            {
                this.mStateStrategy.Sell(mam, slotIndex); 

                TempData["Message"] = "Спасибо, наслаждайтесь вашим напитком !";
            }
            else
            {
                TempData["Message"] = "Недостаточно средств !";
            }

            TempData["CurrentModel"] = mam;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reset()
        {
            TempData["CurrentModel"] = null;

            return RedirectToAction("Index");
        }
    }
}