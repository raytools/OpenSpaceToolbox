using System;
using System.Threading;

namespace OpenSpaceToolbox
{
   public class Rayman2CheckpointExtra : OpenspaceExtraAction
   {
      public Rayman2CheckpointExtra(Rayman2GameManager gameManager) : base(gameManager)
      {
         Name = ShortName = "Set spawn point (CAREFUL)";

         ResurrectionPositionPointer = 0x500590;

         AssemblyNop1Pointer = 0x406495;
         AssemblyNop2Pointer = 0x4064A2;
         AssemblyNop3Pointer = 0x4031CB;

         RespawnPersoPointer = 0x4B7308;
         RespawnPersoPointerPath = new int[] {0xC, 0, 0x4, 0x1C, 0xA90 };
      }

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

         var playerCoords = GameManager.PlayerCoordinates;

         Thread.Sleep(100);

         GameManager.WriteCoordinates(
            playerCoords.Item1, playerCoords.Item2, playerCoords.Item3,
            ResurrectionPositionPointer);

         // GlobalActorModel.DsgVar_29, reset checkpoint object to null
         GameManager.WriteDWord(0, RespawnPersoPointer, RespawnPersoPointerPath);

         Thread.Sleep(50);

         // Deathwarp and immediately put player back
         GameManager.WriteEngineMode(8); // EM_ModePlayerDead

         Thread.Sleep(150);
         GameManager.PlayerCoordinates = playerCoords;
      }
   }
}