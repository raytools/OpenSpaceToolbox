using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace OpenSpaceToolbox
{
   /// <summary>
   /// View model for the Speed Monitor
   /// </summary>
   public class SpeedMonitorWindowViewModel : BaseViewModel
   {
      #region Constructor

      public SpeedMonitorWindowViewModel(GenericSpeedMonitorExtra extra)
      {
         Extra = extra;

         Task.Run(UpdateSpeedTask);
      }

      #endregion

      #region Public Properties

      public float PositionX => _lastPosition.X;
      public float PositionY => _lastPosition.Y;
      public float PositionZ => _lastPosition.Z;

      public float SpeedX => _speed.X;
      public float SpeedY => _speed.Y;
      public float SpeedZ => _speed.Z;
      public float SpeedXY => new Vector2(_speed.X, _speed.Y).Length();
      public float SpeedXYZ => _speed.Length();

      public float AverageSpeedX => _averageSpeed.X;
      public float AverageSpeedY => _averageSpeed.Y;
      public float AverageSpeedZ => _averageSpeed.Z;
      public float AverageSpeedXY => new Vector2(_averageSpeed.X, _averageSpeed.Y).Length();
      public float AverageSpeedXYZ => _averageSpeed.Length();

      public float AverageSpeedDuration { get; set; } = 5.0f;

      public bool Active { get; set; } = true;

      #endregion

      #region Private Properties

      private GenericSpeedMonitorExtra Extra { get; }

      #endregion

      #region Fields

      private Vector3 _speed;
      private Vector3 _averageSpeed;

      private Vector3 _lastPosition;
      private long _lastTime;

      private Dictionary<long, Vector3> _speedHistory;

      #endregion

      #region Public Methods

      public async Task UpdateSpeedTask()
      {
         _speedHistory = new Dictionary<long, Vector3>();

         await Task.Run(async () =>
         {
            while (Active) {
               try {
                  int processHandle = Extra.GameManager.GetProcessHandle(false);

                  if (processHandle >= 0) {

                     var position = Extra.PlayerCoordinates;
                     var newTime = DateTime.Now.Ticks;
                     float timeDelta = (newTime - _lastTime) / (float)TimeSpan.TicksPerSecond;

                     if (timeDelta > 0) {
                        _speed = (position - _lastPosition) / timeDelta;
                        _speedHistory.Add(newTime, _speed);
                     }

                     // Remove old speeds from the history
                     long cutoffTime = newTime - (long)(AverageSpeedDuration * TimeSpan.TicksPerSecond);
                     _speedHistory = _speedHistory.
                        Where(kv => kv.Key > cutoffTime).
                        ToDictionary(kv=>kv.Key, kv=>kv.Value);

                     _averageSpeed = _speedHistory.Aggregate(Vector3.Zero,
                                        (s, v) => s + v.Value) / _speedHistory.Count;

                     _lastPosition = position;
                     _lastTime = DateTime.Now.Ticks;

                     OnPropertyChanged(nameof(PositionX));
                     OnPropertyChanged(nameof(PositionY));
                     OnPropertyChanged(nameof(PositionZ));

                     OnPropertyChanged(nameof(SpeedX));
                     OnPropertyChanged(nameof(SpeedY));
                     OnPropertyChanged(nameof(SpeedZ));
                     OnPropertyChanged(nameof(SpeedXY));
                     OnPropertyChanged(nameof(SpeedXYZ));

                     OnPropertyChanged(nameof(AverageSpeedX));
                     OnPropertyChanged(nameof(AverageSpeedY));
                     OnPropertyChanged(nameof(AverageSpeedZ));
                     OnPropertyChanged(nameof(AverageSpeedXY));
                     OnPropertyChanged(nameof(AverageSpeedXYZ));
                  }

                  await Task.Delay(100);
               } catch (Exception ex) {
                  MessageBox.Show(ex.Message);
               }
            }
         });
      }

      #endregion
   }
}