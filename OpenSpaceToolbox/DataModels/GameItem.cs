using System;

namespace OpenSpaceToolbox
{
    public class GameItem
    {
        public GameItem(string name, Type type)
        {
            Name = name;
            Class = type;
        }

        public string Name { get; }

        public Type Class { get; }
    }
}
