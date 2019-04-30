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
    class IT6_UI_Downwards
    {

        /// <summary>
        /// This SUT test the integration between UI and CookControl
        /// PowerTube and Timer is also included since they are
        /// integration tested with CookControl
        /// </summary>
        #region Objects and SetUp

        private UserInterface _uut;
        private Display _display;
        private Light _light;
        private CookController _cookController;
        private PowerTube _powerTube;
        private Timer _timer;

        private IOutput _output;

        private IButton _buttonPower;
        private IButton _buttonTime;
        private IButton _buttonStartCancel;
        private IDoor _door;

        [SetUp]
        public void sut_initalization()
        {
            _output = Substitute.For<IOutput>();
            _buttonPower = Substitute.For<IButton>();
            _buttonTime = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _light = new Light(_output);
            _display = new Display(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _uut = new UserInterface(_buttonPower, _buttonTime,
            _buttonStartCancel, _door, _display, _light, _cookController);
            _cookController.UI = _uut;
        }

        #endregion


        [Test]
        public void OnPowerPressed_Ready_OutPuts()
        {
            //Default state is READY

            _uut.OnPowerPressed(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 50 W")));
        }
      
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void OnPowerPressed_SetPower_OutPuts(int X)
        {
            //Power pressed X times
            for (int i=0; i < X; i++)
            { 
                _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            _output.ClearReceivedCalls();


            _uut.OnPowerPressed(this,EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: {((X+1) * 50)} W")));
        }

        [Test]
        public void OnTimePressed_SetPower_OutPuts()
        {
            //Set state to SETPOWER
            _uut.OnPowerPressed(this, EventArgs.Empty);
            
            _uut.OnTimePressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 01:00")));

        }

        [Test]
        public void OnTimePressed_SetTime_OutPuts()
        {
            //Set state to SETTIME
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);

            _output.ClearReceivedCalls();
            _uut.OnTimePressed(this,EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 02:00")));
        }

        
        [Test]
        public void OnStartCancel_SetPower_Outputs()
        {
            //Set state to COOKING in order to test light 
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);
            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.ClearReceivedCalls();

            //Set state to SETPOWER
            _uut.OnPowerPressed(this, EventArgs.Empty);


            _uut.OnStartCancelPressed(this,EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Light is turned off")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display cleared")));
      
            //Assert Light = Off, Display = Clear
        }
        [Test]
        public void OnStartCancel_SetTime_Outputs()
        {
            //Set state to SETTIME
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);

            _output.ClearReceivedCalls();
            

            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Light is turned on")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display cleared")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube works with 7,14 %")));

            //Assert Light = On, Display = Clear, Powertube = 7,14 %
        }

        [Test]
        public void OnStartCancel_Cooking_Outputs()
        {
            //Set state to COOKING
            _uut.OnPowerPressed(this, EventArgs.Empty);
            _uut.OnTimePressed(this, EventArgs.Empty);
            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.ClearReceivedCalls();


            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Light is turned on")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display cleared")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube turned off")));
            //Assert Light = Off, Display = Clear, Powertube = Off
        }

    }
}
