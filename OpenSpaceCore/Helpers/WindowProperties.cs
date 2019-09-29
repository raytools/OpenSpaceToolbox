using System.Windows;
using OpenSpaceCore.Helpers.WPF;

namespace OpenSpaceCore.Helpers
{
    public class WindowProperties : BaseViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ResizeMode ResizeMode { get; set; }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
