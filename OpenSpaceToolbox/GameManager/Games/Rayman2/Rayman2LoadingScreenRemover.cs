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

         byte[] nopInstructions = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90};

         GameManager.WriteBytes(nopInstructions, 0x45F85E);
         GameManager.WriteBytes(nopInstructions, 0x4508F0);
         GameManager.WriteBytes(nopInstructions, 0x4043D6);
      }
   }
}