using System;
using System.Threading;

namespace OpenSpaceToolbox
{
   public class Rayman2CheckpointExtra : OpenspaceExtraAction {
      public enum CheckpointMode
      {
         CurrentPosition,
         SavedPosition
      }

      public Rayman2CheckpointExtra(Rayman2GameManager gameManager, CheckpointMode mode) : base(gameManager)
      {
         string modeName = mode switch
         {
            CheckpointMode.CurrentPosition => "(At Current)",
            CheckpointMode.SavedPosition => "(At Saved)",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
         };
         Name = ShortName = $"⚠ Set spawn point {modeName}";
         Mode = mode;

         ResurrectionPositionPointer = 0x500590;

         AssemblyNop1Pointer = 0x406495;
         AssemblyNop2Pointer = 0x4064A2;
         AssemblyNop3Pointer = 0x4031CB;

         RespawnPersoPointer = 0x4B7308;
         RespawnPersoPointerPath = new int[] {0xC, 0, 0x4, 0x1C, 0xA90 };

         Tooltip = "⚠ Warning: these actions modify the program's executable memory and require a game restart to undo!";
      }

      public CheckpointMode Mode { get; set; }

      private int RespawnPersoPointer { get; }
      private int[] RespawnPersoPointerPath { get; }
      
      private int ResurrectionPositionPointer { get; }

      private int AssemblyNop1Pointer { get; }
      private int AssemblyNop2Pointer { get; }
      private int AssemblyNop3Pointer { get; }

      public override void Action()
      {
         int processHandle = GameManager.GetProcessHandle();
         if (processHandle < 0)
            return;

         // (ugly and a bit too permanent)
         // nop the code that resets the resurrection point
         GameManager.WriteBytes(new byte[5] { 0x90, 0x90, 0x90, 0x90, 0x90 }, AssemblyNop1Pointer);
         GameManager.WriteBytes(new byte[5] { 0x90, 0x90, 0x90, 0x90, 0x90 }, AssemblyNop2Pointer);

         /*
          * Code that's NOPed:
          * void fn_vSnifThePlayerIsDead(void)
            {
               -> POS_fn_vSetIdentityMatrix(&g_stEngineStructure.stMainCharacterPosition);
               -> POS_fn_vSetIdentityMatrix(&g_stEngineStructure.stMainCameraPosition);
            ...
         */

         // and nop the code that checks if resurrection is TRUE
         // (so it always executes that part)
         GameManager.WriteBytes(new byte[2] { 0x90, 0x90}, AssemblyNop3Pointer);

         /*
          * Code that's NOPed:
          * void fn_vInitDeadLoop() {
          * ...
          * -> if(g_stEngineStructure.bResurection)
          */

         var currentCoords = Mode switch {
            CheckpointMode.CurrentPosition => GameManager.PlayerCoordinates,
            CheckpointMode.SavedPosition => GameManager.SavedPosition,
            _ => throw new ArgumentOutOfRangeException()
         };

         //CheckpointMode.CurrentPosition ? GameManager.PlayerCoordinates;

         Thread.Sleep(100);

         GameManager.WriteCoordinates(
            currentCoords.Item1, currentCoords.Item2, currentCoords.Item3,
            ResurrectionPositionPointer);

         // GlobalActorModel.DsgVar_29, reset checkpoint object to null
         GameManager.WriteDWord(0, RespawnPersoPointer, RespawnPersoPointerPath);

         Thread.Sleep(50);

         // Deathwarp and immediately put player back
         GameManager.WriteEngineMode(8); // EM_ModePlayerDead

         Thread.Sleep(150);
         GameManager.PlayerCoordinates = currentCoords;
      }
   }
}