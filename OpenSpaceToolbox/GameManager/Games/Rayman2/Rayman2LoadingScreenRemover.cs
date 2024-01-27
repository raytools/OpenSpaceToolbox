using System;
using System.Threading;

namespace OpenSpaceToolbox
{
   public class Rayman2LoadingScreenRemover : OpenspaceExtraAction {

      public Rayman2LoadingScreenRemover(Rayman2GameManager gameManager) : base(gameManager)
      {
         Name = ShortName = $"⚠ Remove loading screens";

         Tooltip = "⚠ Warning: these actions require a game restart to undo!";
      }

      public override void Action()
      {
         int processHandle = GameManager.GetProcessHandle();
         if (processHandle < 0)
            return;

         byte[] retBytes = new byte[] { 0xC3 };

         GameManager.WriteBytes(retBytes, 0x45F7E0);
         GameManager.WriteBytes(retBytes, 0x45F530);
         GameManager.WriteBytes(retBytes, 0x45EED0);
         GameManager.WriteBytes(retBytes, 0x45EDF0);
         GameManager.WriteBytes(retBytes, 0x45EE10);
      }
   }
}