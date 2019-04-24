using System.Runtime.CompilerServices;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Configuration;
using NUnit.Framework.Constraints;

namespace Microwave.Test.Integrations
{
    [TestFixture]
    public class IT1_Powertube_Output
    {
        private IOutput _output;
        private ITimer _timer;
        private IDisplay _display;

        private PowerTube _powerTube;
        private CookController _uut;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _powerTube = new PowerTube(_output);
            _uut = new CookController(_timer, _display, _powerTube);
        }

        [TestCase(90,10)]
        [TestCase(10, 10)]
        public void StartCooking_OutputCorrect(int power, int time)
        {
            _uut.StartCooking(power,time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));
        }

        [TestCase(101, 10)]
        [TestCase(-10, 10)]
        public void StartCooking_PowerOutOfRange(int power, int time)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>_uut.StartCooking(power,time));
        }

        [TestCase(50, 10)]
        public void StartCooking_PowerIsOn(int power, int time)
        {
            _uut.StartCooking(power,time);
            Assert.Throws<System.ApplicationException>(() => _uut.StartCooking(power, time));
        }


        [TestCase(10,10)]
        public void StopCooking_OutputCorrect(int power, int time)
        {
            _uut.StartCooking(power, time);
            _uut.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        }

       // [TestCase(10, 10)]
        //public void OnTimerExpired_Output(int power, int time)
        //{
        //    _uut.StartCooking(power, time);
        //    _uut.OnTimerExpired(this, );
        //    _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        //}
    }
}


