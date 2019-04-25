using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using NSubstitute;
using NUnit.Framework;



namespace Microwave.Test.Integrations
{
    class IT_5_UI_Display_Light
    {
        private UserInterface uut_UserInterface; //This is the top 

        private Display uut_Display; //This is included 
        private Light uut_Light; //This is included

        private ICookController uut_CookController; //This is a stub
        private IPowerTube uut_PowerTube; //This is a stub
        private ITimer uut_Timer; //This is a stub
        private IOutput uut_Output; //This is a stub
        private IButton uut_PowerButton; //This is a stub
        private IButton uut_TimeButton; //This is a stub
        private IButton uut_StartCancelButton; //This is a stub
        private IDoor uut_Door; //This is a stub

        [SetUp]
        public void sut_initalization()
        {
            //Stubs
            uut_CookController = Substitute.For<ICookController>();
            uut_PowerTube = Substitute.For<IPowerTube>();
            uut_Timer = Substitute.For<ITimer>();
            uut_Output = Substitute.For<IOutput>();
            uut_Door = Substitute.For<IDoor>();
            uut_PowerButton = Substitute.For<IButton>();
            uut_TimeButton = Substitute.For<IButton>();
            uut_StartCancelButton = Substitute.For<IButton>();

            //includes
            uut_Display = new Display(uut_Output);
            uut_Light = new Light(uut_Output);

            //Top
            uut_UserInterface = new UserInterface ();
            
        }


    }

}