using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrations
{
    [TestFixture]
    public class IT8_BtnTop_NoStubs
    { 
        private UserInterface _ui;
        private Display _display;
        private Light _light;
        private CookController _cookController;
        private PowerTube _powerTube;
        private Timer _timer;

        private Output _output;

        private Button _buttonPower;
        private Button _buttonTime;
        private Button _buttonStartCancel;
        private IDoor _door;

        [SetUp]
        public void sut_initalization()
        {
            _output = new Output();

            _buttonPower = new Button();
            _buttonTime = new Button();
            _buttonStartCancel = new Button();

            _door = Substitute.For<IDoor>();
            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _light = new Light(_output);
            _display = new Display(_output);
            _ui = new UserInterface(_buttonPower, _buttonTime,
                // send pointer to cookController
                _buttonStartCancel, _door, _display, _light, _cookController);
            // update cookController instance pointer
            _cookController = new CookController(_timer, _display, _powerTube, _ui);
        }


        ///
        /// Alle asserts skal laves om, men jeg aner ikke
        /// hvordan jeg skal asserte output, der ikke er
        /// en substitut. HALP, PLZ
        ///  


        //[Test]
        //public void OnPowerPressed_Ready_OutPuts()
        //{
        //    //Default state is READY
        //    _buttonPower.Press();

        //    _ui.OnPowerPressed(this, EventArgs.Empty);
        //    Assert.That(_ui.
        //        ,Is.EqualTo($"Display shows: 50 W")));


        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Contains($"Display shows: 50 W")));
        //}

        //[TestCase(1)]
        //[TestCase(5)]
        //[TestCase(10)]
        //public void OnPowerPressed_SetPower_OutPuts(int X)
        //{
        //    //Set state to SETPOWER
        //    _buttonPower.Press();

        //    //Power pressed X times
        //    for (int i = 0; i < X; i++)
        //    {
        //        _buttonPower.Press();
        //    }

        //    _output.ClearReceivedCalls();

        //    _buttonPower.Press();

        //    _uut.OnPowerPressed(this, EventArgs.Empty);

        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Display shows: {(X * 50) + 100} W")));
        //}

        //[Test]
        //public void OnTimePressed_SetPower_OutPuts()
        //{
        //    //Set state to SETPOWER
        //    _buttonPower.Press();

        //    _uut.OnTimePressed(this, EventArgs.Empty);
        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Contains($"Display shows: 50 W")));
        //}

        //[Test]
        //public void OnTimePressed_SetTime_OutPuts()
        //{
        //    //Set state to SETTIME
        //    _buttonPower.Press();
        //    _buttonTime.Press();

        //    _uut.OnTimePressed(this, EventArgs.Empty);
        //    // test for default value
        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Contains($"Display shows: 01:00")));
        //}


        //[Test]
        //public void OnStartCancel_SetPower_Outputs()
        //{
        //    //Set state to COOKING in order to test light 
        //    _buttonPower.Press();
        //    _buttonTime.Press();
        //    _buttonStartCancel.Press();

        //    _output.ClearReceivedCalls();

        //    //Set state to SETPOWER
        //    _buttonPower.Press();


        //    _uut.OnStartCancelPressed(this, EventArgs.Empty);

        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Light is turned off")));
        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Display cleared")));

        //    //Assert Light = Off, Display = Clear
        //}
        //[Test]
        //public void OnStartCancel_SetTime_Outputs()
        //{
        //    //Set state to SETTIME
        //    _buttonPower.Press();
        //    _buttonTime.Press();

        //    _output.ClearReceivedCalls();


        //    _uut.OnStartCancelPressed(this, EventArgs.Empty);

        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Light is turned on")));
        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Display cleared")));
        //    //Assert Light = On, Display = Clear
        //}

        //[Test]
        //public void OnStartCancel_Cooking_Outputs()
        //{
        //    //Set state to COOKING
        //    _buttonPower.Press();
        //    _buttonTime.Press();
        //    _buttonPower.Press();

        //    _output.ClearReceivedCalls();


        //    _uut.OnStartCancelPressed(this, EventArgs.Empty);

        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Light is turned on")));
        //    _output.Received().OutputLine(Arg.Is<string>(str =>
        //        str.Equals($"Display cleared")));
        //    //Assert Light = Off, Display = Clear
        //}
    }
}
