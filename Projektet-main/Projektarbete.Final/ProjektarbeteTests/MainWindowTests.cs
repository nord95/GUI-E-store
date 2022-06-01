using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projektarbete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projektarbete.Tests
{
    [TestClass()]
    public class MainWindowTests
    {

        //OrderButton_Click
        [TestMethod()]
        public void CorrectVoucher()
        {
            var voucher = MainWindow.TryVoucherCode("correctVoucher.csv");

            Assert.AreEqual("correctvoucher", voucher[0].Code.ToString());
            Assert.AreEqual(20, voucher[0].Discount);
            Assert.AreEqual(1, voucher.Count);
        }
        [TestMethod()]
        public void SpecialCharInVoucher()
        {
            var voucher = MainWindow.TryVoucherCode("specialCharactersVoucher.csv");
            Assert.AreEqual(0, voucher.Count);
        }
        [TestMethod()]
        public void ToLongVoucher()
        {
            var voucher = MainWindow.TryVoucherCode("toLongVoucher.csv");
            Assert.AreEqual(0, voucher.Count);
        }

        [TestMethod()]
        public void ReadProductFromFileCorrectly()
        {
            string filePath = "correctProducts.csv";
            var test = MainWindow.ReadProducts(filePath);
            Assert.AreEqual("testbild.jpg", test[0].ImagePath.ToString());
            Assert.AreEqual("borste", test[0].Name.ToString());
            Assert.AreEqual("reder ut tovor", test[0].Description.ToString());
            Assert.AreEqual(100, test[0].Price);
            Assert.AreEqual(1, test.Length);
        }
        [TestMethod()]
        public void ReadProductIncorrect()
        {
            string filePath = "incorrectProduct.csv";
            var test = MainWindow.ReadProducts(filePath);

            Assert.AreEqual(0, test.Length);
        }
        [TestMethod()]
        public void TotalAfterDiscountRounded()
        {
            double sum = 1000.53;
            double discount = 40;
            var test = MainWindow.SumAfterDiscount(discount, sum);
            Assert.AreEqual(600.32, test);
        }

    }
}
