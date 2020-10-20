using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ServerApplication
{
    public class ServerButtons
    {
        public Button Button_Start;
        public Button Button_Stop;
        public ServerButtons(Button button_Start, Button button_Stop)
        {
            Button_Start = button_Start;
            Button_Stop = button_Stop;
        }
    }
}
