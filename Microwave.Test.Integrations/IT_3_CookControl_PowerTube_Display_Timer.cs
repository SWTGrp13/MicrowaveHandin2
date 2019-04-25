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
        private Timer _time;
        private IOutput _output;
        private IUserInterface _ui;
        private Display _display;
        private PowerTube _pt;
        private CookController _uut_cc;

        [SetUp]
        public void sut_initalize()
        {
            // Substitute for Stups in sut-testcase 3
            _output = Substitute.For<IOutput>();
            _ui = Substitute.For<IUserInterface>();
            // instantiate includes pr sut-testcast 3
            _display = new Display(_output);
            _time = new MicrowaveOvenClasses.Boundary.Timer();
            _pt = new PowerTube(_output);
            // instantiate TOP for sut-testcase 3
            _uut_cc = new CookController(_time, _display, _pt, _ui);
        }
   
        [TestCase(40,2000)]
        public void testCookControllerTimeTick(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power,time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));
           
            pause.WaitOne(1000+500);
            pause.Set();
            Assert.That(_time.TimeRemaining,Is.EqualTo(1000));
        }
        
        [TestCase(60, 2000)]
        public void testCookControllerTimeTickDisplay(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {power} %")));

            pause.WaitOne(1000+500);
            pause.Set();
            time = time-1000;
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display shows: {time/60}:{time%60}")));
        }

    
        [TestCase(50,2000)]
        public void testCookControllerTimeExpired(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power, time);
            // something is iffy here..
           
            pause.WaitOne(time+1000);
            pause.Set();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube turned off")));
        }
       
        [TestCase(40, -1500)]
        public void testCookControllerNegativeTimeThrowsException(int power, int time)
        {
            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);
        }

    }
}
