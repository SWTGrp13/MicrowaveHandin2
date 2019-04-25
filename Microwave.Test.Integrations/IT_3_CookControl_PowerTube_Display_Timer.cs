using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integrations
{
    class IT_3_CookControl_PowerTube_Display_Timer
    {
        private Timer _uut_time;
        private IOutput _uut_output;
        private IUserInterface _uut_ui;
        private Display _uut_display;
        private PowerTube _uut_pt;
        private CookController _uut_cc;

        [SetUp]
        public void sut_initalize()
        {
            // Substitute for Stups in sut-testcase 3
            _uut_output = Substitute.For<IOutput>();
            _uut_ui = Substitute.For<IUserInterface>();
            // instantiate includes pr sut-testcast 3
            _uut_display = new Display(_uut_output);
            _uut_time = new MicrowaveOvenClasses.Boundary.Timer();
            _uut_pt = new PowerTube(_uut_output);
            // instantiate TOP for sut-testcase 3
            _uut_cc = new CookController(_uut_time, _uut_display, _uut_pt);
        }
      
        [TestCase(40,2000)]
        public void testCookControllerTimeTick(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power,time);
            _uut_output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));
          
            pause.WaitOne(1000);
            pause.Set();
            Assert.That(_uut_time.TimeRemaining,Is.EqualTo(1000));
        }

        [TestCase(50,2000)]
        public void testCookControllerTimeExpired(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power, time);
            pause.WaitOne(2000);
            pause.Set();
            _uut_output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        }

        [TestCase(40, -1500)]
        public void testCookControllerNegativeTimeThrowsException(int power, int time)
        {
            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);
        }

    }
}
