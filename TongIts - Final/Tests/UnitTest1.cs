using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOLayer;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConstrutorTestWithValues_Positive()
        {
            Card target = new Card(Suit.Spades, FaceValue.Three);
            Assert.IsTrue(target.FaceValue == FaceValue.Three);
            Assert.IsTrue(target.Suit == Suit.Spades);
        }
    }
}
