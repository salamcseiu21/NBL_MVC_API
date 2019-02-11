using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBL.Models.Validators;

namespace NBL.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            var result = Validator.ValidateProductBarCode("1025896321547");
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestMethod1()
        {
           var result=Validator.ValidateProductBarCode("012569368lokdk");
            Assert.AreEqual(true, result);
        }
        
    }
}
