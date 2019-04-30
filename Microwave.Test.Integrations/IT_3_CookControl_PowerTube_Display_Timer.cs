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
        private double percentage;

        [SetUp]
        public void sut_initialize()
        {
            // Substitute for Stubs in sut-testcase 3
            _output = Substitute.For<IOutput>();
            _ui = Substitute.For<IUserInterface>();
            // instantiate includes pr sut-testcast 3
            _display = new Display(_output);
            _time = new Timer();
            _pt = new PowerTube(_output);
            // instantiate TOP for sut-testcase 3
            _uut_cc = new CookController(_time, _display, _pt, _ui);
        }

        [TestCase(350, 2)]
        public void testCookControllerTimeTick(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);

            _uut_cc.StartCooking(power, time);


            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube works with {percentage} %")));

            pause.WaitOne(1050);
            pause.Set();

            Assert.That(_time.TimeRemaining, Is.EqualTo(1));

            _time.Stop();
        }

        [TestCase(350, 5)] //Five senconds
        [TestCase(350, 2)] //Two senconds
        public void testCookControllerTimeTickDisplay(int power, int time)
        {
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);
            ManualResetEvent pause = new ManualResetEvent(false);

            _uut_cc.StartCooking(power, time);
           
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube works with {percentage} %")));

            pause.WaitOne(1050);
            pause.Set();
            time = time - 1;
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: 00:{time:D2}")));

            _time.Stop();
        }

        [TestCase(350, -1500)]
        public void testCookControllerNegativeTimeThrowsException(int power, int time)
        {
            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);

            _time.Stop();
        }

        [TestCase(350, 5)]//Five Seconds
        public void testCookControllerTimeExpired(int power, int time)
        {

            ManualResetEvent pause = new ManualResetEvent(false);
            _uut_cc.StartCooking(power, time);
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);

            pause.WaitOne((6000));
            pause.Set();
            _output.Received().OutputLine(Arg.Is<string>(str =>
                 str.Equals($"PowerTube works with {percentage} %")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: 00:00")));

            _time.Stop();
        }


        
        [TestCase(350, 2)]
        public void OnTimerEventCorrectTime(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);

            _uut_cc.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube works with {percentage} %")));
            _output.ClearReceivedCalls();

            pause.WaitOne((1050));
            pause.Set();
            time = time - 1;

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: 00:{time:D2}")));
            _time.Stop();
        }


        [TestCase(350, 5)] //Set time to five second
        public void OnTimerExpireCorrectTime(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);
            _uut_cc.StartCooking(power, time);

            pause.WaitOne(1050);
            pause.Set();

            time = time - 1;

            //Assert that display shows time -1sec
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: 00:04")));
            // Assert time is 00:04

            _time.Stop();
        }

    }
}
