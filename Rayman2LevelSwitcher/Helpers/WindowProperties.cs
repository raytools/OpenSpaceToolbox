using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rayman2LevelSwitcher
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
