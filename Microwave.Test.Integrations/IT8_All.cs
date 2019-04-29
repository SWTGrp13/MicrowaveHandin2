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
            // open door
            _door.Open();
            // assert light on
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Light is turned on")));
        }


        [Test]
        public void TestDoorOpen_SetPower()
        {

            //Open door
            _door.Open();
            _door.Close();
            //State SetPower
            _buttonPower.Press();
            _buttonTime.Press();
            //Assert Light on
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Light is turned on")));
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Light is turned off")));
            _buttonStartCancel.Press();
            //Assert DisplayClear
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display cleared")));
            //Assert powerlevel 50
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"PowerTube works with 7,14 %")));
        }


        [Test]
        public void TestDoorOpen_SetTime()
        {
            //Open door
            _door.Open();
            _door.Close();
            //State SetPower
            _buttonPower.Press();
            //State SetTime
            _buttonTime.Press();
            _door.Open();
           
            //Assert powerlevel 50
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"PowerTube works with 7,14 %")));
            //Assert time 1
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 01:00")));
            //Assert Light on
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Light is turned on")));
            //Assert DisplayClear
            _output.OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display cleared")));
        }

        [Test]
        public void TestOnDoorOpen_Cooking()
        {
            //Set stato to COOKING
            _buttonPower.Press();
            _buttonTime.Press();
            _buttonStartCancel.Press();
            _output.ClearReceivedCalls();
            //Open door
            _door.Open();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals(
                $"PowerTube turned off")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Contains($"Display shows: 01:00")));
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"PowerTube works with 7.14 %"))); //Power == 50
            _output.Received().OutputLine(Arg.Is<string>(str =>
                str.Equals($"Display shows: 50 W")));
        }


        [Test]
        public void TestOnDoorClosed()
        {
            //Set state to DOOROPEN
            _door.Open();
            _output.ClearReceivedCalls();
            _door.Close();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals(
                $"Light is turned off")));
            //Assert light off
        }
    }
}
