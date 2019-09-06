using System.Collections.ObjectModel;

namespace OpenSpaceToolbox
{
    public class GameList
    {
        
        public GameList()
        {
            Games = new ObservableCollection<GameItem>()
            {
                new GameItem("Rayman 2: The Great Escape", typeof(Rayman2GameManager)),
                new GameItem("Donald Duck: Goin' Quackers", typeof(DonaldGameManager))
            };
        }

        public ObservableCollection<GameItem> Games { get; }
    }
}
