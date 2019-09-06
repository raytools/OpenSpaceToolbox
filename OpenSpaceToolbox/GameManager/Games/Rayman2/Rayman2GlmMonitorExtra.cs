using System.Windows;

namespace OpenSpaceToolbox
{
    public class Rayman2GlmMonitorExtra : OpenspaceExtraAction
    {
        public Rayman2GlmMonitorExtra(Rayman2GameManager gameManager) : base(gameManager)
        {
            Name = "GLM Monitor 2000";
            ShortName = "GLM";
            GlmBasePointer = 0x500298;
            GlmOffsets = new[] {0x234, 0x10, 0xC, 0xB0};
        }

        private int GlmBasePointer { get; }
        private int[] GlmOffsets { get; }

        public (float, float, float) GlmCoordinates
        {
            get => GameManager.ReadCoordinates(GlmBasePointer, GlmOffsets);
            set => GameManager.WriteCoordinates(value.Item1, value.Item2, value.Item3, GlmBasePointer, GlmOffsets);
        }

        public override void Action()
        {
            GlmWindow glmWindow = new GlmWindow(new GlmWindowViewModel(this))
            {
                Owner = Application.Current.MainWindow
            };
            glmWindow.Show();
        }
    }
}
