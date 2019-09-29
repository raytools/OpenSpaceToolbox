using OpenSpaceCore.GameManager;

namespace OpenSpaceCore.DataModels
{
    public class GameItem
    {
        public GameItem(string name, GenericGameManager instance)
        {
            Name = name;
            Class = instance;
        }

        public string Name { get; }

        public GenericGameManager Class { get; }
    }
}
