using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OpenSpaceToolbox
{
    /// <summary>
    /// View model for the Progress Array
    /// </summary>
    public class ProgressArrayWindowViewModel : BaseViewModel
    {
        #region Constructor

        public ProgressArrayWindowViewModel(Rayman2ProgressArrayExtra extra)
        {
            Extra = extra;

            LoadProgressCommand = new RelayCommand(LoadProgress);
            SaveProgressCommand = new RelayCommand(SaveProgress);
        }

        #endregion

        #region Commands

        public ICommand LoadProgressCommand { get; }
        public ICommand SaveProgressCommand { get; }

        #endregion

        #region Private Properties

        private Rayman2ProgressArrayExtra Extra { get; }

        #endregion

        #region Public Properties

        public string EditProgressText { get; set; }

        public string SavedProgressText
        {
            get => SavedProgress==null ? string.Empty : string.Join(" ", SavedProgress.Select(b => b.ToString()));
            set
            {
                string[] byteStrings = value.Split(' ');
                byte[] byteArray = new byte[Rayman2ProgressArrayExtra.ProgressArrayLengthBytes];

                if (byteStrings.Length != byteArray.Length) return;

                for(int i=0;i<byteStrings.Length;i++) {
                    if (byte.TryParse(byteStrings[i], out byte byteVal)) {
                        byteArray[i] = byteVal;
                    }

                }

                SavedProgress = byteArray;
            }
        }


        public byte[] SavedProgress { get; set; }

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

        #endregion

    }
}
