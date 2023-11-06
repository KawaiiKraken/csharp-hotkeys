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
        private static List<string> LastListOfMatchingHotkeys = new List<string>();
        private static List<string> ListOfMatchingHotkeys = new List<string>();
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

                CheckIfHotkeyTriggered();
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            currentlyPressedKeys.Remove(e.KeyCode);
            CheckIfHotkeyTriggered(); // in this case it doesnt check anything but resets state so hotkey can be triggered repeatedly
        }

        private List<string> ListTriggeredHotkeys()
        {
            LastListOfMatchingHotkeys = new List<string>(HotkeyListener.ListOfMatchingHotkeys);
            ListOfMatchingHotkeys.Clear();

            for (int i = 0; i < HotkeyList.Count; i++)
            {
                if (HotkeyList[i].Hotkey.TrueForAll(hotkey => currentlyPressedKeys.Contains(hotkey)))
                {
                    ListOfMatchingHotkeys.Add(HotkeyList[i].Name);
                }
            }

            // TODO fix bug where hotkey is triggered every second keypress if one of the keys is held down
            var missingInList1 = ListOfMatchingHotkeys.Except(LastListOfMatchingHotkeys);
            //var missingInList2 = LastListOfMatchingHotkeys.Except(ListOfMatchingHotkeys);
            //var diffBoth = missingInList1.Concat(missingInList2);
            string result = string.Join(", ", missingInList1);
            string result2 = string.Join(", ", ListOfMatchingHotkeys);
            string result3 = string.Join(", ", LastListOfMatchingHotkeys);
            Program.label3.Text = $"current: {result2}, last: {result3}, diff: {result}";
            List<string> diff = new(missingInList1);

            return diff;
        }

        private void CheckIfHotkeyTriggered()
        {
            //string result = string.Join(", ", lastListOfMatchingHotkeys.Select(n => n.ToString()));
            //MessageBox.Show(result);
            List<string> matchingHotkeys = ListTriggeredHotkeys();
            if (matchingHotkeys.Count > 0)
            {
                for (int i = 0; i < matchingHotkeys.Count; i++)
                {
                    HotkeyTriggered?.Invoke(matchingHotkeys[i]);
                }
            }
        }
    }
}
