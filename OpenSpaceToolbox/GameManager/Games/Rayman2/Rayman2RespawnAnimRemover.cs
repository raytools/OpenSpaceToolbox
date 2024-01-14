using System;
using System.Threading;

namespace OpenSpaceToolbox
{
   public class Rayman2RespawnAnimRemover : OpenspaceExtraAction {

      public Rayman2RespawnAnimRemover(Rayman2GameManager gameManager) : base(gameManager)
      {
         Name = ShortName = $"⚠ Remove death+respawn animations";

         Tooltip = "⚠ Warning: these actions require a game restart to undo!";
      }

      public override void Action()
      {
         int processHandle = GameManager.GetProcessHandle();
         if (processHandle < 0)
            return;

         int[] pathDeathAnimNumFrames = new int[] { 0x44, 0x8, 0x0, 0xC, 0x4, 0x20, 0x80 };
         int[] pathSpawnAnimNumFrames = new int[] { 0x3C, 0x0, 0x58, 0x4, 0x38, 0x0, 0x184};

         GameManager.WriteBytes(new byte[1] { 1 }, 0x500560, pathDeathAnimNumFrames);
         GameManager.WriteBytes(new byte[1] { 1 }, 0x500560, pathSpawnAnimNumFrames);
      }
   }
}