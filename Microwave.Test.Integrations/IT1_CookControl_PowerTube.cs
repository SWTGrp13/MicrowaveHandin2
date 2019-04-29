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
    public class IT1_CookControl_PowerTube
    {
        private IOutput _output;
        private ITimer _timer;
        private IDisplay _display;
        private IUserInterface _ui;

        private double percentage;

        private PowerTube _powerTube;
        private CookController _uut;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _ui = Substitute.For<IUserInterface>();
            _powerTube = new PowerTube(_output);
            _uut = new CookController(_timer, _display, _powerTube, _ui);
        }

        [TestCase(630, 10)]
        [TestCase(70, 10)]
        public void StartCooking_OutputCorrect(int power, int time)
        {
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);
            _uut.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {percentage} %")));
        }

        [TestCase(701, 10)]
        [TestCase(-10, 10)]
        public void StartCooking_PowerOutOfRange(int power, int time)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _uut.StartCooking(power, time));
        }

        [TestCase(50, 10)]
        public void StartCooking_PowerIsOn(int power, int time)
        {
            _uut.StartCooking(power, time);
            Assert.Throws<System.ApplicationException>(() => _uut.StartCooking(power, time));
        }


        [TestCase(10, 10)]
        public void StopCooking_OutputCorrect(int power, int time)
        {
            _uut.StartCooking(power, time);
            _uut.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        }

        [Test]
        public void OnTimerExpired_Output()
        {

            _uut.StartCooking(50, 50);

            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"turned off")));
        }
    }
}