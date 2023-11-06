namespace WinFormsApp3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class HotkeyListener
    {
        public event Action<Keys[]> HotkeyTriggered;

        private List<Keys> currentlyPressedKeys = new List<Keys>();
        private List<Keys> hotkeys = new List<Keys>();

        public HotkeyListener(List<Keys> hotkeyList)
        {
            hotkeys = hotkeyList;

            KeyboardHook keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += OnKeyDown;
            keyboardHook.KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!currentlyPressedKeys.Contains(e.KeyCode))
            {
                currentlyPressedKeys.Add(e.KeyCode);

                if (HotkeyMatch())
                {
                    OnHotkeyTriggered();
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            currentlyPressedKeys.Remove(e.KeyCode);
        }

        private bool HotkeyMatch()
        {
            return hotkeys.All(hotkey => currentlyPressedKeys.Contains(hotkey));
        }

        private void OnHotkeyTriggered()
        {
            HotkeyTriggered?.Invoke(hotkeys.ToArray());
        }
    }

}
