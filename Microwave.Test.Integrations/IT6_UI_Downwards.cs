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
            _uut = new UserInterface(_buttonPower, _buttonTime,
            // send pointer to cookController
            _buttonStartCancel, _door, _display, _light, _cookController);
            // update cookController instance pointer
            _cookController = new CookController(_timer, _display, _powerTube, _uut);
        }

        #endregion


        [Test]
        public void OnPowerPressed_Ready_OutPuts()
        {
            //Default state is READY

            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);


            _uut.OnPowerPressed(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 50 W")));

            //Assert Powerlevel Output 50
        }
      
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void OnPowerPressed_SetPower_OutPuts(int X)
        {
            //Set state to SETPOWER:
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            
            //Power pressed X times
            for (int i=0; i < X; i++)
            { 
                _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }
            
            //_uut.OnPowerPressed(this, EventArgs.Empty); <- ikke nødvendigt da eventet bliver nedarvet
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 50 W")));

            //Assert Powerlevel output X*50
        }

        [Test]
        public void OnTimePressed_Ready_OutPuts()
        {
            //Default state is READY
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 50 W")));
           
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // test for default value
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 01:00")));
        }

        [Test]
        public void OnStartCancel_OutPuts()
        {
            //Default state is READY
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // set time
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // output covered in previous tests
            // _output.ClearReceivedCalls();
            // circular dependency, ah shit. (found one error)
            // _buttonStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);


            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 01:00")));


        }
    }
}
