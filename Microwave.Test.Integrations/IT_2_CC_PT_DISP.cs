using System;
using System.Diagnostics;
using System.Threading;
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
      
        private ITimer _time;
        private IOutput _output;
        private Display _display;
        private PowerTube _pt;
        private CookController _uut_cc;
        private double percentage;

        [SetUp]
        public void sut_initalize()
        {
            // Substitute for Stups in sut-testcase 2
            _time = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();
            // instantiate includes pr sut-testcast 2
            _display = new Display(_output);
            _pt = new PowerTube(_output);
            // instantiate TOP for sut-testcase 2
            _uut_cc = new CookController(_time, _display, _pt);
        }
       
       
        [TestCase(50, 1000)]
        public void TestCookControllerAndDisplay(int power, int time)
        {
            percentage = Math.Round(((Convert.ToDouble(power) / 700) * 100), 2);
            _uut_cc.StartCooking(power, time);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {percentage} %")));
         
            _time.TimerTick += Raise.Event();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display shows: 00:00")));


        }

        [TestCase(70, 2000)]
        public void testCookControllerStop(int power, int time)
        {
            _uut_cc.StartCooking(power, time);

            _uut_cc.Stop();

            _output.Received().OutputLine(Arg.Is<string>(str => 
                str.Equals($"PowerTube turned off")));
        }

       
    }

}
