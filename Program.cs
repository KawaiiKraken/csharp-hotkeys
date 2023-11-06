
namespace WinFormsApp3
{
    internal static class Program
    {
        private static Label label1 = new Label();
        private static Label label2 = new Label();
        public static Label label3 = new Label();
        private static HotkeyManager hotkeyManager = new HotkeyManager();
        private static List<HotkeyListener.HotkeyStruct> HotkeyList = new List<HotkeyListener.HotkeyStruct>();
        private static int triggerCount = 0;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form form = new Form();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            label3.Dock = DockStyle.Top;
            label3.TextAlign = ContentAlignment.TopLeft;
            label3.Text = "default text 3";
            form.Controls.Add(label3);

            label2.Dock = DockStyle.Top;
            label2.TextAlign = ContentAlignment.TopLeft;
            label2.Text = "default text 2";
            form.Controls.Add(label2);

            label1.Dock = DockStyle.Top;
            label1.TextAlign = ContentAlignment.TopLeft;
            label1.Text = "default text 1";
            form.Controls.Add(label1);

            Button asyncButton1 = new Button();
            asyncButton1.Text = "Set Hotkey";
            asyncButton1.Click += async (sender, e) => await AsyncButton1ClickHandler(sender, e);
            asyncButton1.Width = 200;
            asyncButton1.Height = 30;
            asyncButton1.Top = 70;
            asyncButton1.Left = 50;
            form.Controls.Add(asyncButton1);

            Button asyncButton2 = new Button();
            asyncButton2.Text = "Listen for Hotkey";
            asyncButton2.Click += AsyncButton2ClickHandler;
            asyncButton2.Width = 200;
            asyncButton2.Height = 30;
            asyncButton2.Top = 120;
            asyncButton2.Left = 50;
            form.Controls.Add(asyncButton2);

            Application.Run(form);
        }
        private static async Task AsyncButton1ClickHandler(object sender, EventArgs e)
        {
            label1.Text = $"recording hotkey...";
            List<Keys> hotkeys = await hotkeyManager.AddHotkey();
            HotkeyList.Add(new HotkeyListener.HotkeyStruct("one", hotkeys));
            //hotkeys = new List<Keys>(hotkeys);
            string result = string.Join(", ", HotkeyList[0].Hotkey.Select(n => n.ToString()));
            label1.Text = $"set hotkey {HotkeyList[0].Name} to: {result}";
        }
        private static void AsyncButton2ClickHandler(object sender, EventArgs e)
        {
            string result = string.Join(", ", HotkeyList[0].Hotkey.Select(n => n.ToString()));
            label2.Text = $"listening for hotkey: {result}";
            HotkeyListener hotkeyListener = new(HotkeyList);
            hotkeyListener.HotkeyTriggered += (hotkeyName) =>
            {
                string result = string.Join(", ", HotkeyList[0].Hotkey.Select(n => n.ToString()));
                triggerCount += 1;
                label2.Text = $"hotkey {hotkeyName} triggered: {result}, {triggerCount} times.";
            };
        }
    }
}