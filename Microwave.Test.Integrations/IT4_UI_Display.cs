using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrations
{
    [TestFixture]
    public class IT4_UI_Display
    {

        #region Objects and SetUp

        private ICookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private ILight _light;
        private IOutput _output;

        private IButton _buttonPower;
        private IButton _buttonTime;
        private IButton _buttonStartCancel;
        private IDoor _door;

        private Display _display;
        private UserInterface _uut;

        [SetUp]
        public void SetUp()
        {
            _cookController = Substitute.For<ICookController>();
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();
            _light = Substitute.For<ILight>();
            _output = Substitute.For<IOutput>();

            _buttonPower = Substitute.For<IButton>();
            _buttonTime = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();


            _display = new Display(_output);
            _uut = new UserInterface(_buttonPower,_buttonTime,_buttonStartCancel,_door,_display,_light,_cookController);
        }
        #endregion

        #region PowerButtonPressed

        [Test]
        public void OnPowerPressed_Ready()
        {
            //PowerPressed 
            //Assert output Show Power $"Display shows: {power} W"
            
        }

        [Test]
        public void OnPowerPressed_SetPower()
        {
            //PowerPressed State SETPOWER
            //Assert output Power $"Display shows: {power} W"
        }
        #endregion

        #region TimeButtonPressed
        [Test]
        public void OnTimePressed_SetPower()
        {
            //PowerPressed State SETPOWER
            //Assert output Power $"Display shows: {min:D2}:{sec:D2}"
        }

        [Test]
        public void OnTimePressed_SetTime()
        {
            //PowerPressed State SETTIME
            //Assert output Power $"Display shows: {min:D2}:{sec:D2}"
        }

        #endregion

        #region StartButtonPressed

        [Test]
        public void OnStartCancelPressed_SetPower()
        {
            //PowerPressed State SETPOWER
            //Assert output Power $"Display cleared"
        }

        [Test]
        public void OnStartCancelPressed_SetTime()
        {
            //PowerPressed State SETPOWER
            //Assert output Power $"Display cleared"
        }

        [Test]
        public void OnStartCancelPressed_Cooking()
        {
            //PowerPressed State COOKING
            //Assert output Power $"Display cleared"
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

        public void OnDoorOpened_Cooking()
        {
            //PowerPressed State Cooking
            //Assert output Power $"Display cleared"
        }

        #endregion

        #region OnDoorClosed

        public void OnDoorClosed_DoorOpen()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            //PowerPressed State DOOROPEN
            //Assert output Power $"Display cleared"
        }

        #endregion

        [Test]
        public void CookingIsDone()
        {
            
          //  _uut.CookingIsDone();


       //     _output.Received().OutputLine(Arg.Is<string>(str => str.Equals($"Display cleared")));
            //When CookingIsDone
            //Assert $"Display cleared"

        }

    }
}
