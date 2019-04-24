using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrations
{
    class IT_3_CookControl_PowerTube_Display_Timer
    {
        private ITimer _uut_time;
        private IOutput _uut_output;
        private Display _uut_display;
        private PowerTube _uut_pt;
        private CookController _uut_cc;

        [SetUp]
        public void sut_initalize()
        {
            // Substitute for Stups in sut-testcase 3
            _uut_output = Substitute.For<IOutput>();
            // instantiate includes pr sut-testcast 3
            _uut_display = new Display(_uut_output);
            _uut_time = new Timer();
            _uut_pt = new PowerTube(_uut_output);
            // instantiate TOP for sut-testcase 3
            _uut_cc = new CookController(_uut_time, _uut_display, _uut_pt);
        }


        [TestCase(40, -1500)]
        public void testCookControllerNegativeTimeThrowsException(int power, int time)
        {
            Assert.That(() => _uut_cc.StartCooking(power, time), Throws.Exception);
        }

    }
}
