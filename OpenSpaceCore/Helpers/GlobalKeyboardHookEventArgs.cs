using System.ComponentModel;

namespace OpenSpaceCore.Helpers
{
    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public GlobalKeyboardHookEventArgs(GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData, GlobalKeyboardHook.KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }

        public GlobalKeyboardHook.KeyboardState KeyboardState { get; }

        public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; }

    }
}