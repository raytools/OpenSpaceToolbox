using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rayman2LevelSwitcher
{
    /// <summary>
    /// View model for the GLM Monitor 2000
    /// </summary>
    public class GlmWindowViewModel : BaseViewModel
    {
        #region Constructor

        public GlmWindowViewModel(Rayman2GlmMonitorExtra extra)
        {
            Extra = extra;

            LoadGlmPosCommand = new RelayCommand(LoadGlmPos);
            SaveGlmPosCommand = new RelayCommand(SaveGlmPos);
            ToggleLivePreviewCommand = new RelayCommand(ToggleLivePreview);
        }

        #endregion

        #region Commands

        public ICommand LoadGlmPosCommand { get; }
        public ICommand SaveGlmPosCommand { get; }
        public ICommand ToggleLivePreviewCommand { get; }

        #endregion

        #region Private Properties

        private Rayman2GlmMonitorExtra Extra { get; }

        private float _glmX;
        private float _glmY;
        private float _glmZ;

        #endregion

        #region Public Properties

        public bool LivePreviewEnabled { get; set; }

        public float SavedXPosition { get; set; }
        public float SavedYPosition { get; set; }
        public float SavedZPosition { get; set; }

        public float GlmX
        {
            get => _glmX;
            set
            {
                GlmDeltaX = value - _glmX;
                _glmX = value;
            }
        }

        public float GlmY
        {
            get => _glmY;
            set
            {
                GlmDeltaY = value - _glmY;
                _glmY = value;
            }
        }

        public float GlmZ
        {
            get => _glmZ;
            set
            {
                GlmDeltaZ = value - _glmZ;
                _glmZ = value;
            }
        }

        public float GlmDeltaX { get; set; }
        public float GlmDeltaY { get; set; }
        public float GlmDeltaZ { get; set; }

        #endregion

        #region Private Methods

        private void LoadGlmPos()
        {
            Extra.GlmCoordinates = (SavedXPosition, SavedYPosition, SavedZPosition);
        }

        private void SaveGlmPos()
        {
            var coords = Extra.GlmCoordinates;
            SavedXPosition = coords.Item1;
            SavedYPosition = coords.Item2;
            SavedZPosition = coords.Item3;
        }

        private void GetGlmPos()
        {
            var coords = Extra.GlmCoordinates;
            GlmX = coords.Item1;
            GlmY = coords.Item2;
            GlmZ = coords.Item3;
        }

        private void ClearGlmPos()
        {
            GlmX = 0;
            GlmY = 0;
            GlmZ = 0;
            GlmDeltaX = 0;
            GlmDeltaY = 0;
            GlmDeltaZ = 0;
        }

        private void ToggleLivePreview()
        {
            if (LivePreviewEnabled)
                Task.Run(RefreshGlmPos);
        }

        #endregion

        #region Public Methods
        public async Task RefreshGlmPos()
        {
            await Task.Run(async () =>
            {
                while (LivePreviewEnabled)
                {
                    try
                    {
                        int processHandle = Extra.GameManager.GetProcessHandle(false);

                        if (processHandle >= 0)
                        {
                            GetGlmPos();
                        }

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                ClearGlmPos();
            });
        }

        #endregion
        
    }
}
