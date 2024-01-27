using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSpaceToolbox.GameManager {
   public enum EnumEngineMode : byte{
		Invalid = 0,
		StartingProgram = 1,                  // first init
		StoppingProgram = 2,                  // last desinit
		EnterGame = 3,                        // init game loop
		QuitGame = 4,                         // desinit game loop
		EnterLevel = 5,                       // init level loop
		ChangeLevel = 6,                      // desinit level loop
		DeadLoop = 7,                         // init dead loop
		PlayerDead = 8,                       // desinit dead loop
		Playing = 9,                          // playing game
	}
}
