using System.Collections.ObjectModel;
using OpenSpaceCore.DLL;
using OpenSpaceCore.GameManager;

namespace OpenSpaceCore.DataModels
{
    public class GameList
    {
        public GameList()
        {
            Games = new ObservableCollection<GameItem>();

            foreach (string dll in Libraries.GetDllList(ProgramPaths.Games))
            {
                GenericGameManager dllClass = Libraries.LoadDll<GenericGameManager>(dll);

                Games.Add(new GameItem(dllClass.Name, dllClass));
            }
        }

        public ObservableCollection<GameItem> Games { get; }
    }
}