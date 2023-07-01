#define ENABLE_PROGRESSARRAY_LOGGING

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OpenSpaceToolbox.Helpers;

namespace OpenSpaceToolbox
{
   /// <summary>
   /// View model for the Progress Array
   /// </summary>
   public class ProgressArrayWindowViewModel : BaseViewModel
   {
      #region Constants

      // Golden fists are stored with 2 bits, giving 4 options (0,1,2,3 fists)
      public const int GoldenFistStartIndex = 961;
      public const int GoldenFistEndIndex = 962;

      public class FlagItem
      {
         private Rayman2ProgressArrayExtra extra;
         public int Index;
         public string Name { get; set; }

         public string IndexString => "ID " + Index.ToString();

         public bool flagCache;

         public FlagItem(Rayman2ProgressArrayExtra extra, int index, string name)
         {
            this.extra = extra;
            Index = index;
            Name = name;
            flagCache = false;
         }

         public void UpdateFlagCache()
         {
            flagCache = extra.ProgressArray.IsBitSet(Index-1);
         }

         public override string ToString()
         {
            return $"#{Index} {Name} ({(flagCache ? "TRUE" : "FALSE")})";
         }
      }

      public List<FlagItem> FlagNames = new List<FlagItem>();

      #endregion

      #region Constructor

      public ProgressArrayWindowViewModel(Rayman2ProgressArrayExtra extra)
      {
         Extra = extra;

         LoadProgressCommand = new RelayCommand(LoadProgress);
         SaveProgressCommand = new RelayCommand(SaveProgress);

         ResetCagesCommand = new RelayCommand(ResetCagesCollected);
         ResetLumsCommand = new RelayCommand(ResetLumsCollected);

         SetCagesCommand = new RelayCommand(SetCagesCollected);
         SetLumsCommand = new RelayCommand(SetLumsCollected);

         SetMegashootCommand = new RelayCommand(() =>
         {
            var array = Extra.ProgressArray;
            array.SetBits(GoldenFistStartIndex, GoldenFistEndIndex, true);
            Extra.ProgressArray = array;
         });

         ResetMegashootCommand = new RelayCommand(() =>
         {
            var array = Extra.ProgressArray;
            array.SetBits(GoldenFistStartIndex, GoldenFistEndIndex, false);
            Extra.ProgressArray = array;
         });

         RefreshCustomFlagCommand = new RelayCommand(RefreshCustomFlag);

         FlagNames = new List<FlagItem>()
         {
            new FlagItem(Extra, 953, "Saved ly in Fairy Glade"),
            new FlagItem(Extra, 954, "Carmen freed"),
            new FlagItem(Extra, 957, "The gate at the end of Canopy 2 opened"),
            new FlagItem(Extra, 958, "Opened Globox's cage in Canopy 2"),
            new FlagItem(Extra, 959, "The gate at the start of Canopy 3 opened"),
            new FlagItem(Extra, 960, "Woods of Light portal Unlocked"),
            new FlagItem(Extra, 961, "Fairy Glade Portal Unlocked"),
            new FlagItem(Extra, 964, "Marshes of Awakening portal unlocked"),
            new FlagItem(Extra, 966, "Cave of Bad Dreams portal unlocked"),
            new FlagItem(Extra, 967, "Bayou Portal unlocked"),
            new FlagItem(Extra, 969, "Walk of Life Portal unlocked"),
            new FlagItem(Extra, 970, "Water and Ice Portal unlocked"),
            new FlagItem(Extra, 972, "Menhir Hills Portal unlocked"),
            new FlagItem(Extra, 975, "Echoing Caves portal unlocked"),
            new FlagItem(Extra, 976, "Canopy portal unlocked"),
            new FlagItem(Extra, 979, "Whale bay portal unlocked"),
            new FlagItem(Extra, 981, "Stone and Fire portal unlocked"),
            new FlagItem(Extra, 985, "Precipice portal unlocked"),
            new FlagItem(Extra, 988, "Top of the World portal unlocked"),
            new FlagItem(Extra, 990, "Sanctuary of Rock and Lava portal unlocked"),
            new FlagItem(Extra, 992, "Walk of Power portal unlocked"),
            new FlagItem(Extra, 993, "Beneath the Sanctuary of rock and lava portal unlocked"),
            new FlagItem(Extra, 1000, "Iron Mountains portal unlocked"),
            new FlagItem(Extra, 1002, "Prison Ship portal"),
            new FlagItem(Extra, 1005, "Crow's nest portal"),
            new FlagItem(Extra, 1006, "Wipe_10/Astro_20 portal unlocked (unused)"),
            new FlagItem(Extra, 1007, "Tomb of the Ancients portal unlocked"),
            new FlagItem(Extra, 1008, "Lum Checkpoint 1 (Water and Ice) passed"),
            new FlagItem(Extra, 1010, "Lum Checkpoint 2 (Stone and Fire) passed"),
            new FlagItem(Extra, 1011, "Lum Checkpoint 2 (Beneath Rock and Lava) passed"),
            new FlagItem(Extra, 1012, "Lum Checkpoint 4 (Iron Mountains) passed"),
            new FlagItem(Extra, 1091, "Watched Woods of Light intro"),
            new FlagItem(Extra, 1092, "Watched Woods of Light baby globox cutscene"),
            new FlagItem(Extra, 1095, "Blue shots, Grab purple lums (hangon)"),
            new FlagItem(Extra, 1101, "Clark told Rayman about the Cave of Bad Dreams"),
            new FlagItem(Extra, 1102, "Clark healed and watched the cutscene"),
            new FlagItem(Extra, 1109, "Watched the cutscene at the start of canopy 3"),
            new FlagItem(Extra, 1123, "Elixir of Life obtained"),
            new FlagItem(Extra, 1133, "watched woods of light teensie cutscene"),
            new FlagItem(Extra, 1143, "orange shots, fist charge power (glowfist)"),
            new FlagItem(Extra, 1159, "Foutch defeated in Beneath Rock and Lava"),
            new FlagItem(Extra, 1171, "Clark destroyed the first wall"),
            new FlagItem(Extra, 1172, "Clark destroyed the second wall"),
            new FlagItem(Extra, 1175, "Woods of Light murfy 1"),
            new FlagItem(Extra, 1176, "nomoremurfy_1176"),
            new FlagItem(Extra, 1177, "Woods of Light murfy 2"),
            new FlagItem(Extra, 1178, "nomoremurfy_1178"),
            new FlagItem(Extra, 1179, "nomoremurfy_1179"),
            new FlagItem(Extra, 1180, "nomoremurfy_1180"),
            new FlagItem(Extra, 1181, "Whale Bay 2 murfy"),
            new FlagItem(Extra, 1182, "nomoremurfy_1182"),
            new FlagItem(Extra, 1183, "nomoremurfy_1183"),
            new FlagItem(Extra, 1184, "nomoremurfy_1184"),
            new FlagItem(Extra, 1188, "NoMovies cheat"),
            new FlagItem(Extra, 1399, "Think ability (First yellow lum)"),
         };

#if ENABLE_PROGRESSARRAY_LOGGING
         Task.Run(RefreshArray);
#endif
      }

      #endregion

      #region Commands

      public ICommand LoadProgressCommand { get; }
      public ICommand SaveProgressCommand { get; }

      public ICommand ResetCagesCommand { get; }
      public ICommand ResetLumsCommand { get; }

      public ICommand SetCagesCommand { get; }
      public ICommand SetLumsCommand { get; }

      public ICommand SetMegashootCommand { get; }
      public ICommand ResetMegashootCommand { get; }

      public ICommand RefreshCustomFlagCommand { get; }

      #endregion

      #region Private Properties

      private Rayman2ProgressArrayExtra Extra { get; }

      #endregion

      #region Public Properties

      public string EditProgressText { get; set; }

      public string SavedProgressText
      {
         get => SavedProgress == null ? string.Empty : string.Join(" ", SavedProgress.Select(b => b.ToString()));
         set
         {
            if (value == null) return;
            string[] byteStrings = value.Split(' ');
            byte[] byteArray = new byte[Rayman2ProgressArrayExtra.ProgressArrayLengthBytes];

            if (byteStrings.Length != byteArray.Length) return;

            for (int i = 0; i < byteStrings.Length; i++) {
               if (byte.TryParse(byteStrings[i], out byte byteVal)) {
                  byteArray[i] = byteVal;
               }
            }

            SavedProgress = byteArray;
         }
      }


      public byte[] SavedProgress { get; set; }

      public bool LoadProgressOnReload
      {
         get => _loadProgressOnReload;
         set
         {
            Extra.GameManager.OnReloadLevel -= LoadProgress;
            if (value) {
               Extra.GameManager.OnReloadLevel += LoadProgress;
            }

            _loadProgressOnReload = value;
         }
      }

      public bool CustomFlagToggle
      {
         get
         {
            if (CustomFlagSelection != null) {
               CustomFlagSelection.UpdateFlagCache();
               return CustomFlagSelection.flagCache;
            }

            return false;
         }
         set
         {
            if (CustomFlagSelection != null) {
               var array = Extra.ProgressArray;
               array.SetBits(CustomFlagSelection.Index-1, CustomFlagSelection.Index-1, value);
               Extra.ProgressArray = array;
               
               CustomFlagSelection.UpdateFlagCache();
               var oldSelection = CustomFlagSelection;
               CustomFlagSelection = null;
               CustomFlagSelection = oldSelection;
               OnPropertyChanged(nameof(CustomFlagSelection));
            }
         }
      }

      public FlagItem[] CustomFlagItems => FlagNames.ToArray();
      public FlagItem CustomFlagSelection { get; set; }

      public bool CustomFlagSelectionNotNull =>CustomFlagSelection!=null;

      private bool _loadProgressOnReload;

      #endregion

      #region Private Methods

      private void LoadProgress()
      {
         SavedProgressText = EditProgressText;
         Extra.ProgressArray = SavedProgress;
      }

      private void SaveProgress()
      {
         SavedProgress = Extra.ProgressArray;
         EditProgressText = SavedProgressText;
         OnPropertyChanged(nameof(SavedProgressText));
      }

      private void ResetLumsCollected()
      {
         SetLums(false);
      }

      private void ResetCagesCollected()
      {
         SetCages(false);
      }

      private void SetLumsCollected()
      {
         SetLums(true);
      }

      private void SetCagesCollected()
      {
         SetCages(true);
      }

      private void SetLums(bool set)
      {
         var bytes = Extra.ProgressArray;
         bytes.SetBits(0, 800, set);
         bytes.SetBits(1200, 1400, set);
         Extra.ProgressArray = bytes;
      }

      private void SetCages(bool set)
      {
         var bytes = Extra.ProgressArray;
         bytes.SetBits(840, 919, set);
         Extra.ProgressArray = bytes;
      }

      private void RefreshCustomFlag()
      {
         CustomFlagSelection.UpdateFlagCache();
         var oldSelection = CustomFlagSelection;
         CustomFlagSelection = null;
         CustomFlagSelection = oldSelection;
      }

      #endregion

      #region Public Methods

#if ENABLE_PROGRESSARRAY_LOGGING


      public async Task RefreshArray()
      {
         byte[] oldBytes = new byte[Rayman2ProgressArrayExtra.ProgressArrayLengthBytes];

         await Task.Run(async () =>
         {
            while (true) {
               try {
                  int processHandle = Extra.GameManager.GetProcessHandle(false);

                  if (processHandle >= 0) {
                     byte[] newBytes = Extra.ProgressArray;

                     for (int i = 0; i < Rayman2ProgressArrayExtra.ProgressArrayLengthBytes; i++) {
                        byte oldByte = oldBytes[i];
                        byte newByte = newBytes[i];

                        for (int j = 0; j < 8; j++) {
                           bool oldBit = (oldByte & (1 << j)) != 0;
                           bool newBit = (newByte & (1 << j)) != 0;

                           if (oldBit != newBit) {
                              Debug.WriteLine(
                                 $"Progress Array Change: Flag {(i * 8) + j} changed from {(oldBit ? "TRUE" : "FALSE")} to {(newBit ? "TRUE" : "FALSE")}");
                           }
                        }
                     }

                     oldBytes = newBytes;
                  }

                  await Task.Delay(100);
               } catch (Exception ex) {
                  MessageBox.Show(ex.Message);
               }
            }
         });
      }

#endif

      public void RefreshFlagList()
      {
         if (Extra.ProgressArray == null) {
            return;
         }
         foreach (var f in FlagNames) {
            f.UpdateFlagCache();
         }
      }

      #endregion
   }
}