using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.ExceptionServices;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;



namespace Microwave.Test.Integrations
{
    [TestFixture]
    public class IT_5_UI_Display_Light
    {
        #region Objects and SetUp

        private UserInterface _uut;

        private Display _display; 
        private Light _light; 

        private ICookController _cookController; 
        private IPowerTube _powerTube; 
        private ITimer _timer; 
        private IOutput _output; 

        private IButton _buttonPower; 
        private IButton _buttonTime; 
        private IButton _buttonStartCancel; 
        private IDoor _door; 

        [SetUp]
        public void sut_initalization()
        {
        
            _cookController = Substitute.For<ICookController>();
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();

            _buttonPower = Substitute.For<IButton>();
            _buttonTime = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();

            _light = new Light(_output);
            _display = new Display(_output);
            _uut = new UserInterface(_buttonPower, _buttonTime,
                _buttonStartCancel, _door, _display, _light, _cookController);
        }

        #endregion


        #region PowerButtonPressed

        [Test]
        public void OnPowerPressed_Ready()
        {
            //State is READY
            _uut.OnPowerPressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"Display shows:")));

        }

        [Test]
        public void OnPowerPressed_SetPower()
        {
            //Set state to SETPOWER:
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _uut.OnPowerPressed(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"Display shows:")));
            //Assert output Power $"Display shows: {power} W"
        }
        #endregion

        #region TimeButtonPressed
        [TestCase(5)]
        [TestCase(1)]
        [TestCase(20)]
        public void OnTimePressed_SetPower(int time)
        {
            //Set state to SETPOWER:
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            for (int i = 0; i < time; i++)
            {
                _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            _uut.OnTimePressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display shows: {time:D2}:{0:D2}")));
        }

        [TestCase(5)]
        [TestCase(1)]
        [TestCase(20)]
        public void OnTimePressed_SetTime(int time)
        {

            //Set state to SETTIME
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            for (int i = 0; i < time; i++)
            {
                _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }
            _buttonStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _uut.OnTimePressed(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display shows: {time:D2}:{0:D2}")));

            //Assert output Power $"Display shows: {min:D2}:{sec:D2}"
        }

        #endregion

        #region StartButtonPressed

        [Test]
        public void OnStartCancelPressed_SetPower()
        {
            //Set state to SETPOWER:
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display cleared")));
            //Assert output Power $"Display cleared"
        }

        [Test]
        public void OnStartCancelPressed_SetTime()
        {
            //Set state to SETTIME
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _uut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display cleared")));
            //Assert output Power $"Display cleared"
        }

        [Test]
        public void OnStartCancelPressed_Cooking()
        {
            //Set state to COOKING
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);


            _uut.OnStartCancelPressed(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"Display cleared")));

        }
        #endregion

        #region OnDoorOpened

        [Test]
        public void OnDoorOpened_Ready()
        {
            //PowerPressed State READY
            //Assert output Power $"Display cleared"
        }
        public void OnDoorOpened_SetPower()
        {
            //PowerPressed State SETPOWER
            //Assert output Power $"Display cleared"
        }

        public void OnDoorOpened_SetTime()
        {
            //PowerPressed State SETTIME
            //Assert output Power $"Display cleared"
        }

        #endregion

        #region OnDoorClosed
        [Test]
        public void OnDoorClosed_DoorOpen()
        {
            //Set state to DOOROPEN
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);


            _output.ClearReceivedCalls();

            _uut.OnDoorClosed(this,EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(
                str => str.Equals($"Light is turned off")));
            //Assert output Power "Light is turned off"
        }

        #endregion

        [Test]
        public void CookingIsDone()
        {
            
            //Set state to COOKING
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _buttonStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            
            
            _output.ClearReceivedCalls();

            _uut.CookingIsDone();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals(
                    $"Light is turned off")));


            //Assert $"Light is turned on"
        }
    }

}