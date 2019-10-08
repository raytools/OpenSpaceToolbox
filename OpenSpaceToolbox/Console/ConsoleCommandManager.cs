using System;
using System.Collections.Generic;
using OpenSpaceCore.GameManager;
using OpenSpaceCore.Helpers;

namespace OpenSpaceToolbox
{
    public class ConsoleCommandManager
    {
        #region Constructor

        public ConsoleCommandManager(GenericGameManager gameManager)
        {
            GameManager = gameManager;

            Commands = new Dictionary<string, ConsoleCommand>
            {
                {"help", args => $"Available commands:\n{string.Join(" ", Commands.Keys)}"},
                {"version", args => $"{AppInfo.CallingName} v{AppInfo.CallingVersion} {GameManager.ExecName}"},
                {
                    "PlayerCoordinates", args =>
                    {
                        var oldCoordinates = GameManager.PlayerCoordinates;

                        if (args.Length < 3 || args.Length > 3)
                            return oldCoordinates.ToString();

                        var newCoordinates = (float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]));
                        GameManager.PlayerCoordinates = newCoordinates;
                        return $"Old coords: {oldCoordinates.ToString()}\nNew coords: {newCoordinates.ToString()}";
                    }
                }
            };
        }

        #endregion

        #region Properties

        private GenericGameManager GameManager { get; }

        public IDictionary<string, ConsoleCommand> Commands { get; }

        #endregion

        #region Methods

        public delegate string ConsoleCommand(params string[] args);

        public string Execute(string name, params string[] args)
        {
            try
            {
                if (Commands.ContainsKey(name))
                    return Commands[name](args);
            }
            catch (Exception e)
            {
                return e.GetType().ToString();
            }

            return "Command not found";
        }

        #endregion
    }
}