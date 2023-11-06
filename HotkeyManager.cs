namespace WinFormsApp3
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class HotkeyManager
    {
        private KeyboardHook keyboardHook;
        private List<Keys> activeKeys = new List<Keys>();
        private TaskCompletionSource<List<Keys>> hotkeyCompletionSource;

        public HotkeyManager()
        {
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += OnKeyDown;
            keyboardHook.KeyUp += OnKeyUp;
        }

        public async Task<List<Keys>> AddHotkey()
        {
            if (hotkeyCompletionSource != null)
            {
                throw new InvalidOperationException("A hotkey is already in progress.");
            }

            hotkeyCompletionSource = new TaskCompletionSource<List<Keys>>();
            activeKeys.Clear();

            return await hotkeyCompletionSource.Task;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (hotkeyCompletionSource != null)
            {
                if (!activeKeys.Contains(e.KeyCode))
                {
                    activeKeys.Add(e.KeyCode);
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (hotkeyCompletionSource != null)
            {
                if (!hotkeyCompletionSource.Task.IsCompleted)
                {
                    hotkeyCompletionSource.SetResult(activeKeys);
                }
                ResetHotkey();
            }
        }

        private void ResetHotkey()
        {
            hotkeyCompletionSource = null;
            activeKeys.Clear();
        }
    }
}
