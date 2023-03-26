using System.Numerics;
using System.Windows;

namespace OpenSpaceToolbox
{
    public class GenericSpeedMonitorExtra : OpenspaceExtraAction
    {
        public GenericSpeedMonitorExtra(OpenspaceGameManager gameManager) : base(gameManager)
        {
            Name = "Speed Monitor";
            ShortName = "Speed Monitor";
        }

        public Vector3 PlayerCoordinates
        {
            get
            {
                var playerCoords = GameManager.PlayerCoordinates;

                return new Vector3(
                    playerCoords.Item1,
                    playerCoords.Item2,
                    playerCoords.Item3
                );
            }
        }

        public override void Action()
        {
            SpeedMonitorWindow speedWindow = new SpeedMonitorWindow(new SpeedMonitorWindowViewModel(this))
            {
                Owner = Application.Current.MainWindow
            };
            speedWindow.Show();
        }
    }
}
