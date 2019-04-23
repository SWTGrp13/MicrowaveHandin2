using System;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void YesAndYes() {
            Assert.That(true,Is.True);
        }
    }
}
