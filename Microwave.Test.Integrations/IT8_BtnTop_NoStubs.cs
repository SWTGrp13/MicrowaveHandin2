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
            _output = Substitute.For<Output>(); 

            _buttonPower = new Button();
            _buttonTime = new Button();
            _buttonStartCancel = new Button();

            _door = Substitute.For<IDoor>();
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
        public void TestDoorOpen()
        {
            _door.Opened += Raise.Event();

             
        }
    }
}
