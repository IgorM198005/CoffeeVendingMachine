using System;
using System.Linq;
using NUnit.Framework;
using CoffeeVendingMachine.v0;
using CoffeeVendingMachine.v0.Controllers;
using CoffeeVendingMachine.v0.DataStorage;
using CoffeeVendingMachine.v0.Model;
using Moq;
using System.Web.Mvc;

namespace CoffeeVendingMachine.v0.Tests
{
    [TestFixture]
    public class Tests
    {
        private static ManAndMachine ForCoinSelectionStrategy()
        {
            return new ManAndMachine
            {
                CashIn = new MoneySlot[]
                {
                    new MoneySlot {  Nominal = 10, Count = 10},
                    new MoneySlot {  Nominal = 5, Count = 10},
                    new MoneySlot {  Nominal = 2, Count = 10},
                    new MoneySlot {  Nominal = 1, Count = 10}
                },

                Purse = new MoneySlot[]
                {
                    new MoneySlot {  Nominal = 10, Count = 10},
                    new MoneySlot {  Nominal = 5, Count = 10},
                    new MoneySlot {  Nominal = 2, Count = 10},
                    new MoneySlot {  Nominal = 1, Count = 10}
                }
            };
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCoinSelectionStrategya(int index)
        {
            ManAndMachine mam = ForCoinSelectionStrategy();

            CoinSelectionStrategy cs = new CoinSelectionStrategy();

            cs.CashBack(mam, 18);

            Assert.AreEqual(9, mam.CashIn[index].Count);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCoinSelectionStrategyb(int index)
        {
            ManAndMachine mam = ForCoinSelectionStrategy();

            CoinSelectionStrategy cs = new CoinSelectionStrategy();

            cs.CashBack(mam, 18);

            Assert.AreEqual(11, mam.Purse[index].Count);
        }

        private static ManAndMachine ForStateStategy()
        {
            return new ManAndMachine
            {
                AmountInCashIn = 1180,

                AmountInPurse = 1180,

                CashIn = new MoneySlot[]
                {
                    new MoneySlot {  Nominal = 10, Count = 10},
                    new MoneySlot {  Nominal = 5, Count = 10},
                    new MoneySlot {  Nominal = 2, Count = 10},
                    new MoneySlot {  Nominal = 1, Count = 10}
                },

                Purse = new MoneySlot[]
                {
                    new MoneySlot {  Nominal = 10, Count = 10},
                    new MoneySlot {  Nominal = 5, Count = 10},
                    new MoneySlot {  Nominal = 2, Count = 10},
                    new MoneySlot {  Nominal = 1, Count = 10}
                },

                Goods = new Shelf []
                {
                    new Shelf {  Goods = "g1", Price = 13, Count = 10 },
                    new Shelf {  Goods = "g2", Price = 24, Count = 10 },
                    new Shelf {  Goods = "g3", Price = 35, Count = 10 },
                    new Shelf {  Goods = "g4", Price = 46, Count = 10 }
                } 
            };
        }

        [Test]
        public void TestStateStategyPushMoney()
        {
            ManAndMachine mam = ForStateStategy();

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);

            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            int counts00 = mam.CashIn.Sum(x => x.Count);

            int counts01 = mam.Purse.Sum(x => x.Count);

            mass.PushMoneyToMachine(mam, 1);
            
            Assert.AreEqual(1185, mam.AmountInCashIn, "AmountInCashIn");

            Assert.AreEqual(1175, mam.AmountInPurse, "AmountInPurse");

            Assert.AreEqual(5, mam.Reciept, "Reciept");

            Assert.AreEqual(9, mam.Purse[1].Count, "Purse");

            Assert.AreEqual(11, mam.CashIn[1].Count, "Purse");

            int counts10 = mam.CashIn.Sum(x => x.Count);

            int counts11 = mam.Purse.Sum(x => x.Count);

            Assert.AreEqual(counts00, counts10-1, "count0");

            Assert.AreEqual(counts01, counts11 + 1, "count1");
        }

        [Test]
        public void TestStateStategyCanRevert1()
        {
            ManAndMachine mam = ForStateStategy();

            mam.Reciept = 17;

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);
            
            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            Assert.AreEqual(true, mass.CanRevert(mam));
        }

        [Test]
        public void TestStateStategyCanRevert2()
        {
            ManAndMachine mam = ForStateStategy();

            mam.Reciept = 0;

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);

            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            Assert.AreEqual(false, mass.CanRevert(mam));
        }

        [Test]
        public void TestStateStategyRevertMoney()
        {
            ManAndMachine mam = ForStateStategy();

            mam.Reciept = 17; 

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);

            mcs.Setup(x => x.CashBack(mam, 17)); 

            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            mass.RevertMoneyToPurse(mam); 

            Assert.AreEqual(1163, mam.AmountInCashIn, "AmountInCashIn");

            Assert.AreEqual(1197, mam.AmountInPurse, "AmountInPurse");

            Assert.AreEqual(0, mam.Reciept, "Reciept");

            mcs.Verify(x => x.CashBack(mam, 17), Times.Once);
        }

        [Test]
        public void TestStateStategyCanSell1()
        {
            ManAndMachine mam = ForStateStategy();

            mam.Reciept = 3;

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);
            
            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            Assert.AreEqual(false, mass.CanSell(mam, 1));
        }

        [Test]
        public void TestStateStategyCanSell2()
        {
            ManAndMachine mam = ForStateStategy();

            mam.Reciept = 300;

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);

            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            Assert.AreEqual(true, mass.CanSell(mam, 1));
        }

        [Test]
        public void TestStateStategySell()
        {
            ManAndMachine mam = ForStateStategy();

            int goodsCountBefor = mam.Goods.Sum(x => x.Count);

            mam.Reciept = 50;

            var mcs = new Mock<ICoinSelectionStrategy>(MockBehavior.Strict);

            mcs.Setup(x => x.CashBack(mam, 26));

            ManAndMachineStateStrategy mass = new ManAndMachineStateStrategy(mcs.Object);

            mass.Sell(mam, 1);

            Assert.AreEqual(1154, mam.AmountInCashIn, "AmountInCashIn");

            Assert.AreEqual(1206, mam.AmountInPurse, "AmountInPurse");

            Assert.AreEqual(0, mam.Reciept, "Reciept");

            Assert.AreEqual(9, mam.Goods[1].Count, "mam.Goods[1].Count");

            int goodsCountAfter = mam.Goods.Sum(x => x.Count);

            Assert.AreEqual(goodsCountBefor-1, goodsCountAfter, "goodsCount");

            mcs.Verify(x => x.CashBack(mam, 26), Times.Once);
        }

        [Test]
        public void TestControllerPushMoney()
        {
            ManAndMachine mam = new ManAndMachine();

            var mcs = new Mock<IManAndMachineStateStrategy>(MockBehavior.Strict);

            mcs.Setup(x => x.PushMoneyToMachine(mam, 1));

            var mcr = new Mock<IManAndMachineRepository>(MockBehavior.Strict);

            mcr.Setup(x => x.Deserialize(string.Empty)).Returns(mam); 

            var home = new HomeController(mcr.Object, mcs.Object);

            var view = home.PushMoney(string.Empty, 1);

            Assert.IsInstanceOf<RedirectToRouteResult>(view);

            mcs.Verify(x => x.PushMoneyToMachine(mam, 1), Times.Once);

            mcr.Verify(x => x.Deserialize(string.Empty), Times.Once);
        }

        
        [Test]
        public void TestControllerRevertMoney()
        {
            ManAndMachine mam = new ManAndMachine();

            var mcs = new Mock<IManAndMachineStateStrategy>(MockBehavior.Strict);

            mcs.Setup(x => x.RevertMoneyToPurse(mam));

            mcs.Setup(x => x.CanRevert(mam)).Returns(true); 

            var mcr = new Mock<IManAndMachineRepository>(MockBehavior.Strict);

            mcr.Setup(x => x.Deserialize(string.Empty)).Returns(mam);

            var home = new HomeController(mcr.Object, mcs.Object);

            var view = home.RevertMoney(string.Empty);

            Assert.IsInstanceOf<RedirectToRouteResult>(view);

            mcs.Verify(x => x.CanRevert(mam));

            mcs.Verify(x => x.RevertMoneyToPurse(mam), Times.Once);

            mcr.Verify(x => x.Deserialize(string.Empty), Times.Once);
        }
        
        [Test]
        public void TestControllerSell()
        {
            ManAndMachine mam = new ManAndMachine();

            var mcs = new Mock<IManAndMachineStateStrategy>(MockBehavior.Strict);

            mcs.Setup(x => x.Sell(mam, 1));

            mcs.Setup(x => x.CanSell(mam, 1)).Returns(true);

            var mcr = new Mock<IManAndMachineRepository>(MockBehavior.Strict);

            mcr.Setup(x => x.Deserialize(string.Empty)).Returns(mam);

            var home = new HomeController(mcr.Object, mcs.Object);

            var view = home.Sell(string.Empty, 1);

            Assert.IsInstanceOf<RedirectToRouteResult>(view);

            mcs.Verify(x => x.CanSell(mam, 1));

            mcs.Verify(x => x.Sell(mam, 1), Times.Once);

            mcr.Verify(x => x.Deserialize(string.Empty), Times.Once);
        }
        
        [Test]
        public void TestControllerReset()
        {
            var mcs = new Mock<IManAndMachineStateStrategy>(MockBehavior.Strict);
            
            var mcr = new Mock<IManAndMachineRepository>(MockBehavior.Strict);

            var home = new HomeController(mcr.Object, mcs.Object);

            var view = home.Reset();

            Assert.IsInstanceOf<RedirectToRouteResult>(view);
        }

        
        [Test]
        public void TestControllerIndex()
        {
            ManAndMachine mam = new ManAndMachine();

            var mcs = new Mock<IManAndMachineStateStrategy>(MockBehavior.Strict);

            var mcr = new Mock<IManAndMachineRepository>(MockBehavior.Strict);

            mcr.Setup(x => x.GetInitialState()).Returns(mam);

            mcr.Setup(x => x.Serialize(mam)).Returns(string.Empty);

            var home = new HomeController(mcr.Object, mcs.Object);

            var view = home.Index();

            Assert.IsInstanceOf<ViewResult>(view);

            Assert.AreEqual((view as ViewResult).Model, mam);
        }        
    }
}
