using System;
using System.Diagnostics;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrations
{
    [TestFixture]
    class IT_2_CC_PT_DISP
    {
      
        private ITimer _uut_time;
        private IOutput _uut_output;
        private Display _uut_display;
        private PowerTube _uut_pt;
        private CookController _uut_cc;

        [SetUp]
        public void sut_initalize()
        {
            // Substitute for Stups in sut-testcase 2
            _uut_time = Substitute.For<ITimer>();
            _uut_output = Substitute.For<IOutput>();
            // instantiate includes pr sut-testcast 2
            _uut_display = new Display(_uut_output);
            _uut_pt = new PowerTube(_uut_output);
            // instantiate TOP for sut-testcase 2
            _uut_cc = new CookController(_uut_time, _uut_display, _uut_pt);
        }
       
        [TestCase(50, 1000)]
        [TestCase(70, 2000)]
        public void TestCookControllerAndDisplay(int power, int time)
        {
            _uut_cc.StartCooking(power, time);

            _uut_output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));

            var span = TimeSpan.FromSeconds(time);

            _uut_display.ShowTime(span.Minutes, span.Seconds);

            _uut_output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display shows: {time/60}:{time%60}")));
        }

        [TestCase(70, 2000)]
        public void testCookControllerStop(int power, int time)
        {
            _uut_cc.StartCooking(power, time);

            _uut_cc.Stop();

            _uut_output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        }

        [TestCase(40, 1000)]
        public void testCookControllerDoubleStartThrowsException(int power, int time)
        {
            _uut_cc.StartCooking(power, time);

            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);
        }

        [TestCase(10, 1000)]
        [TestCase(70, 1000)]
        public void testCookControllerStartStopStartDoesNotThrowException(int power, int time)
        {
            _uut_cc.StartCooking(power, time);

            _uut_cc.Stop();

            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Nothing);
        }

        [TestCase(-40, 1000)]
        public void testCookControllerNegativePowerThrowsException(int power, int time)
        {
            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);
        }

    }

}
