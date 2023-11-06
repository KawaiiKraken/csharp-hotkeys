
namespace WinFormsApp3
{
    internal static class Program
    {
        private static Label label1 = new Label();
        private static Label label2 = new Label();
        private static List<Keys> hotkeys = new List<Keys>();
        private static HotkeyManager hotkeyManager = new HotkeyManager();
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

            label2.Dock = DockStyle.Top;
            label2.TextAlign = ContentAlignment.TopLeft;
            label2.Text = "default text 2";
            form.Controls.Add(label2);

            label1.Dock = DockStyle.Top;
            label1.TextAlign = ContentAlignment.TopLeft;
            label1.Text = "default text 1";
            form.Controls.Add(label1);

            Button asyncButton1 = new Button();
            asyncButton1.Text = "Async Button1";
            asyncButton1.Click += async (sender, e) => await AsyncButton1ClickHandler(sender, e);
            asyncButton1.Width = 200;
            asyncButton1.Height = 30;
            asyncButton1.Top = 50;
            asyncButton1.Left = 50;
            form.Controls.Add(asyncButton1);

            Button asyncButton2 = new Button();
            asyncButton2.Text = "Async Button2";
            asyncButton2.Click += AsyncButton2ClickHandler;
            asyncButton2.Width = 200;
            asyncButton2.Height = 30;
            asyncButton2.Top = 100;
            asyncButton2.Left = 50;
            form.Controls.Add(asyncButton2);

            Application.Run(form);
        }
        private static async Task AsyncButton1ClickHandler(object sender, EventArgs e)
        {
            label1.Text = $"recording hotkey...";
            hotkeys = await hotkeyManager.AddHotkey();
            hotkeys = new List<Keys>(hotkeys);
            string result = string.Join(", ", hotkeys.Select(n => n.ToString()));
            label1.Text = $"hotkey pressed: {result}";
        }
        private static void AsyncButton2ClickHandler(object sender, EventArgs e)
        {
            string result = string.Join(", ", hotkeys.Select(n => n.ToString()));
            label2.Text = $"listening for hotkey: {result}";
            HotkeyListener hotkeyListener = new(hotkeys);
            hotkeyListener.HotkeyTriggered += (hotkey) =>
            {
                string result = string.Join(", ", hotkeys.Select(n => n.ToString()));
                triggerCount += 1;
                label2.Text = $"hotkey triggered: {result}, {triggerCount} times.";
            };
        }
    }
}