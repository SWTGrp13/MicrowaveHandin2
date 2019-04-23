using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrations
{
    [TestFixture]
    class T1_Powertube_Output
    {
        private IOutput _output;
        private PowerTube _uut;


        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<Output>();
            _uut = new PowerTube(_output);
        }

        [TestCase(10)]
        [TestCase(99)]
        public void TurnOnPowerTube_Allowed(int power)
        {
            _uut.TurnOn(power);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));
        }

    }
}
