
using Newtonsoft.Json;

namespace WinFormsApp3
{
    internal static class Program
    {
        private static Label label1 = new Label();
        private static Label label2 = new Label();
        public  static Label label3 = new Label();
        private static Label label4 = new Label();
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
            form.Height = 600;

            label4.Dock = DockStyle.Top;
            label4.TextAlign = ContentAlignment.TopLeft;
            label4.Text = "default text 4";
            form.Controls.Add(label4);

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
            asyncButton1.Top = 300;
            asyncButton1.Left = 50;
            form.Controls.Add(asyncButton1);

            Button Button2 = new Button();
            Button2.Text = "Listen for Hotkey";
            Button2.Click += Button2ClickHandler;
            Button2.Width = 200;
            Button2.Height = 30;
            Button2.Top = 350;
            Button2.Left = 50;
            form.Controls.Add(Button2);

            Button Button3 = new Button();
            Button3.Text = "Write Config";
            Button3.Click += Button3ClickHandler;
            Button3.Width = 200;
            Button3.Height = 30;
            Button3.Top = 400;
            Button3.Left = 50;
            form.Controls.Add(Button3);

            Button Button4 = new Button();
            Button4.Text = "Read Config";
            Button4.Click += Button4ClickHandler;
            Button4.Width = 200;
            Button4.Height = 30;
            Button4.Top = 450;
            Button4.Left = 50;
            form.Controls.Add(Button4);

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
        private static void Button2ClickHandler(object sender, EventArgs e)
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
        private static void Button3ClickHandler(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(HotkeyList, Formatting.Indented);
            label4.Text = json;
            File.WriteAllText("hotkeys.json", json, System.Text.Encoding.UTF8);
        }
        private static void Button4ClickHandler(object sender, EventArgs e)
        {
            string jsonFromFile = File.ReadAllText("hotkeys.json", System.Text.Encoding.UTF8);
            HotkeyList = JsonConvert.DeserializeObject<List<HotkeyListener.HotkeyStruct>>(jsonFromFile);
            string result = string.Join(", ", HotkeyList[0].Hotkey.Select(n => n.ToString()));
            label4.Text = $"hotkey {HotkeyList[0].Name} content: {result}";
        }
    }
}