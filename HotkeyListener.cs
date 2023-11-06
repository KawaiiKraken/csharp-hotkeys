namespace WinFormsApp3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class HotkeyListener
    {
        // TODO make it return list of strings for matching hotkeys, not just the first one
        public event Action<string> HotkeyTriggered;

        private List<Keys> currentlyPressedKeys = new List<Keys>();
        private static List<HotkeyStruct> HotkeyList = new List<HotkeyStruct>();
        private static int i;
        public struct HotkeyStruct
        {
            public string Name;
            public List<Keys> Hotkey;

            public HotkeyStruct(string name, List<Keys> hotkey)
            {
                Name = name;
                Hotkey = new List<Keys>(hotkey);
            }
        }

        public HotkeyListener(List<HotkeyStruct> hotkeyList)
        {
            HotkeyList = hotkeyList;

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
            bool result = false;
            for (i = 0; i < HotkeyList.Count; i++)
            {
                if (HotkeyList[i].Hotkey.All(hotkey => currentlyPressedKeys.Contains(hotkey)))
                {
                    result = true; break;
                }
            }
            return result;
        }

        private void OnHotkeyTriggered()
        {
            HotkeyTriggered?.Invoke(HotkeyList[i].Name);
        }
    }

}
