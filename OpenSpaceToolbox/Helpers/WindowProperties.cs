using System.Windows;

namespace OpenSpaceToolbox
{
    public class WindowProperties : BaseViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ResizeMode ResizeMode { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetMinSize(int width, int height)
        {
            MinWidth = width;
            MinHeight = height;
        }
    }
}
