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
    public class IT8_All
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
            _output = Substitute.For<Output>(); 

            _buttonPower = new Button();
            _buttonTime = new Button();
            _buttonStartCancel = new Button();

            _door = new Door();
            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _light = new Light(_output);
            _display = new Display(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _ui = new UserInterface(_buttonPower, _buttonTime,
             _buttonStartCancel, _door, _display, _light, _cookController);
            _cookController.UI = _ui;

        }

        [Test]
        public void TestDoorOpen_ready()
        {
            _door.Opened += Raise.Event();

            //State Ready
            //Open door
            //Assert Light on
        }


        [Test]
        public void TestDoorOpen_SetPower()
        {
            //State SetPower
            //Open door
            //Assert Light on
            //Assert DisplayClear
            //Assert powerlevel 50
        }


        [Test]
        public void TestDoorOpen_SetTime()
        {
            //State Settime
            //Open door
            //Assert powerlevel 50
            //Assert time 1
            //Assert Light on
            //Assert DisplayClear
        }

        [Test]
        public void TestDoorOpen_Cooking()
        {
            //State SetPower
            //Open door
            //Assert cooking stopped
            //Assert powerlevel 50
            //Assert time = 1
        }


        [Test]
        public void TestOnDoorClosed()
        {
            //State door open
            //Close door
            //Assert light off
        }



    }
}
