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
    class IT_7_BtnIsTop
    {
        private UserInterface _uut;
        private Display _display;
        private Light _light;
        private CookController _cookController;
        private PowerTube _powerTube;
        private Timer _timer;

        private IOutput _output;

        private Button _buttonPower;
        private Button _buttonTime;
        private Button _buttonStartCancel;
        private IDoor _door;

        [SetUp]
        public void sut_initalization()
        {
            _output = Substitute.For<IOutput>();

            _buttonPower = new Button();
            _buttonTime = new Button();
            _buttonStartCancel = new Button();

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


    }
}
