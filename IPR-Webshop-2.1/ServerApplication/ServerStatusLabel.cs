using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ServerApplication
{
    public class ServerStatusLabel
    {
        private Label label;
        private Ellipse indicator;
        public ServerStatusLabel(Label label, Ellipse indicator)
        {
            this.label = label;
            this.indicator = indicator;
        }
        public void SetStatus(ServerStates serverState)
        {
            switch (serverState)
            {
                case ServerStates.Init:
                    indicator.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case ServerStates.Idle:
                    indicator.Fill = new SolidColorBrush(Colors.Orange);
                    break;
                case ServerStates.Running:
                    indicator.Fill = new SolidColorBrush(Colors.Green);
                    break;
                case ServerStates.Stopped:
                    indicator.Fill = new SolidColorBrush(Colors.Red);
                    break;
                case ServerStates.Error:
                    indicator.Fill = new SolidColorBrush(Colors.White);
                    break;
            }
            this.label.Content = "State: " + serverState.ToString();
        }
    }

    public enum ServerStates
    {
        Running,
        Stopped,
        Init,
        Idle,
        Error
    }
}
