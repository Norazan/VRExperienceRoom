using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using VRExperienceRoom.Serializables;
using VRExperienceRoom.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace VRExperienceRoom
{
    public partial class Form : System.Windows.Forms.Form
    {
        private enum LogType
        {
            LOG = 0,
            ERROR = 1,
            PORT = 2
        }

        private const int MaxTimeCharacterInput = 2;

        private static Scheduler scheduler = null;

        private bool started = false;
        public DateTime timerStart;
        public bool inCountdown = false;
        private bool nonNumberEntered = false;

        private delegate void UpdateLogsCallback(LogType logType, string text, string portName = null);

        public Form()
		{
            Thread.CurrentThread.Name = "MainGUI";
            InitializeComponent();
            ScanCOMPorts();
        }

        private void ScanCOMPorts()
        {
            UpdateLogs(LogType.LOG, "Scanning COM ports for Arduino's...");
            IOHandler.Instance.ScanForArduinos();
            if (IOHandler.Instance.ports.Count > 0)
            {
                string comports = "";
                foreach (SerialPort p in IOHandler.Instance.ports)
                {
                    p.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    comports += " " + p.PortName;
                }
                PortSelector.Items.Clear();
                PortSelector.Items.AddRange(IOHandler.Instance.ports.Select(x => x.PortName).ToArray());
                PortSelector.SelectedIndex = 0;
                PortSelector.Enabled = true;
                TabWindow.Enabled = true;
                UpdateLogs(LogType.LOG, "Connection established with Arduino's on the following COM ports:" + comports);
            }
            else
            {
                PortSelector.Items.Clear();
                PortSelector.Items.Add("No Arduino's were found. Scan again");
                PortSelector.SelectedIndex = 0;
                PortSelector.Enabled = false;
                TabWindow.Enabled = false;
                UpdateLogs(LogType.LOG, "No Arduino's could be found");
            }
        }

        private void UpdateLogs(LogType logType, string text, string portName = null)
        {
            if ((ConsoleWindowCreate.InvokeRequired && ConsoleWindowRun.InvokeRequired) || Thread.CurrentThread.Name != "MainGUI")
            {
                UpdateLogsCallback d = new UpdateLogsCallback(UpdateLogs);

                Invoke(d, new object[] { logType, text, portName });
            }
            else
            {
                switch (logType)
                {
                    case LogType.LOG:
                        ConsoleWindowCreate.AppendText("[Log] " + text + "\n");
                        ConsoleWindowRun.AppendText("[Log] " + text + "\n");
                        break;
                    case LogType.ERROR:
                        ConsoleWindowCreate.AppendText("[ERROR] " + text + "\n");
                        ConsoleWindowRun.AppendText("[ERROR] " + text + "\n");
                        break;
                    case LogType.PORT:
                        ConsoleWindowCreate.AppendText("[" + portName + "] " + text);
                        ConsoleWindowRun.AppendText("[" + portName + "] " + text);
                        break;
                }
            }
        }

        public void StopTimer()
        {
            IOHandler.Instance.StopAllDevices();
            ProgramTimer.Stop();
            TimerLabel.Text = "00:00:00";
            TimerMS.Text = "000";
            TimerButton.Text = "Start";
            TimerStatus.Text = "Stopped";
            TimerStatus.ForeColor = System.Drawing.Color.Red;
            started = false;
        }

        public void SendSettings(SerialPort port, string WindRPM, bool heat, bool scent)
        {
            if (!IOHandler.Instance.CheckExistingConnection(port))
            {
                UpdateLogs(LogType.ERROR, "The following port cannot establish connection with Arduino: " + port.PortName);
                if (ProgramTimer.Enabled)
                {
                    StopTimer();
                    UpdateLogs(LogType.ERROR, "right now its:" + scheduler.settings_index);
                }
                ScanCOMPorts();
            }
            else
            {
                IOHandler.Instance.WriteSettings(port, WindRPM, heat, scent);
            }
        }

        #region Events and Listeners
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string readString = sp.ReadLine();
            UpdateLogs(LogType.PORT, readString, sp.PortName);
        }

		private void AddSettingButton_Click(object sender, EventArgs e)
		{
            string hours = HoursInput.Text.Contains("H") ? "0" : HoursInput.Text;
            string minutes = MinutesInput.Text.Contains("M") ? "0" : MinutesInput.Text;
            string seconds = SecondsInput.Text.Contains("S") ? "0" : SecondsInput.Text;

            if (int.Parse(hours) > 23)
                hours = "23";
            if (int.Parse(minutes) > 59)
                minutes = "59";
            if (int.Parse(seconds) > 59)
                seconds = "59";

            ListViewItem lvi = new ListViewItem(PortSelector.SelectedItem.ToString());
            lvi.SubItems.Add(hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0'));
            lvi.SubItems.Add(WindRPMInput.Text);
            lvi.SubItems.Add(HeatInput.Checked ? "On" : "Off");
            lvi.SubItems.Add(ScentInput.Checked ? "On" : "Off");
            var existingLvi = SettingsListCreate.Items.Cast<ListViewItem>().FirstOrDefault(
                x => x.SubItems[0].Text == lvi.SubItems[0].Text &&
                x.SubItems[1].Text == lvi.SubItems[1].Text);
            if (existingLvi == null)
            {
                SettingsListCreate.Items.Add(lvi);
                List<ListViewItem> tempCollection = SettingsListCreate.Items.Cast<ListViewItem>().OrderBy(x => DateTime.Parse(x.SubItems[1].Text).TimeOfDay).ToList();
                SettingsListCreate.Items.Clear();
                SettingsListCreate.Items.AddRange(tempCollection.ToArray());
            }
            else
            {
                SettingsListCreate.Items[existingLvi.Index] = lvi;
            }
        }

        private void RemoveSettingButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem x in SettingsListCreate.SelectedItems)
            {
                SettingsListCreate.Items.Remove(x);
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            List<DeviceSettings> settings = new List<DeviceSettings>();
            foreach(ListViewItem x in SettingsListCreate.Items)
            {
                DeviceSettings setting = new DeviceSettings();
                setting.Port = x.SubItems[0].Text;
                setting.Timestamp = TimerUtil.TimestampToMs(x.SubItems[1].Text);
                setting.WindRPM = x.SubItems[2].Text;
                setting.Heat = x.SubItems[3].Text;
                setting.Mist = x.SubItems[4].Text;
                settings.Add(setting);
            }
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON File (*.json)|*.json";
            saveFileDialog.Title = "Export config file...";
            saveFileDialog.ShowDialog();

            if(saveFileDialog.FileName != "")
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        } 

        private void TimerButton_Click(object sender, EventArgs e)
        {
            if(started)
            {
                StopTimer();
            }
            else
            {
                scheduler.settings_index = 0;
                timerStart = DateTime.Now;
                TimerButton.Text = "Stop";
                TimerStatus.Text = "Started";
                TimerStatus.ForeColor = System.Drawing.Color.Green;
                started = true;
                if (Countdown.Checked)
                {
                    inCountdown = true;
                    TimerStatus.Text = "Countdown";
                    TimerStatus.ForeColor = System.Drawing.Color.Yellow;
                    TimerLabel.ForeColor = System.Drawing.Color.Gray;
                    TimerMS.ForeColor = System.Drawing.Color.Gray;
                }
                ProgramTimer.Start();
            }
        }

        private void ProgramTimer_Tick_UI(object sender, EventArgs e)
        {
            if (inCountdown)
            {
                TimeSpan countdown = DateTime.Now - timerStart - new TimeSpan(0, 0, 5);
                if (countdown.TotalMilliseconds > 0)
                {
                    TimerStatus.Text = "Started";
                    TimerStatus.ForeColor = System.Drawing.Color.Green;
                    TimerLabel.ForeColor = System.Drawing.Color.Black;
                    TimerMS.ForeColor = System.Drawing.Color.Black;
                    timerStart = DateTime.Now;
                    inCountdown = false;
                }
                TimerLabel.Text = countdown.ToString("hh':'mm':'ss");
                TimerMS.Text = countdown.ToString("fff");
            }
            else
            {
                TimeSpan duration = DateTime.Now - timerStart;
                TimerLabel.Text = duration.ToString("hh':'mm':'ss");
                TimerMS.Text = duration.ToString("fff");
            }
        }

        private void HoursInput_TextChanged(object sender, EventArgs e)
        {
            if(HoursInput.TextLength == HoursInput.MaxLength)
            {
                MinutesInput.Select();
            }
        }

        private void HoursInput_Enter(object sender, EventArgs e)
        {
            if(HoursInput.Text == "H")
            {
                HoursInput.Text = "";
                HoursInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void HoursInput_Leave(object sender, EventArgs e)
        {
            if (HoursInput.Text == "")
            {
                HoursInput.Text = "H";
                HoursInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void MinutesInput_TextChanged(object sender, EventArgs e)
        {
            if (MinutesInput.TextLength == MinutesInput.MaxLength)
            {
                SecondsInput.Select();
            }
        }

        private void MinutesInput_Enter(object sender, EventArgs e)
        {
            if (MinutesInput.Text == "M")
            {
                MinutesInput.Text = "";
                MinutesInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void MinutesInput_Leave(object sender, EventArgs e)
        {
            if (MinutesInput.Text == "")
            {
                MinutesInput.Text = "M";
                MinutesInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void SecondsInput_Enter(object sender, EventArgs e)
        {
            if (SecondsInput.Text == "S")
            {
                SecondsInput.Text = "";
                SecondsInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void SecondsInput_Leave(object sender, EventArgs e)
        {
            if (SecondsInput.Text == "")
            {
                SecondsInput.Text = "S";
                SecondsInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TestSettingsButton_Click(object sender, EventArgs e)
        {
            SerialPort currentPort = IOHandler.Instance.ports.Where(x => x.PortName == PortSelector.SelectedItem.ToString()).First();
            SendSettings(currentPort, WindRPMInput.Text, HeatInput.Checked, ScentInput.Checked);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON File (*.json)|*.json";
            openFileDialog.Title = "Import  config file...";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    string json = sr.ReadToEnd();
                    List<DeviceSettings> currentSettings = JsonConvert.DeserializeObject<List<DeviceSettings>>(json);
                    SettingsListRun.Items.Clear();
                    foreach (DeviceSettings x in currentSettings)
                    {
                        ListViewItem lvi = new ListViewItem(x.Port);
                        lvi.SubItems.Add(TimerUtil.MsToTimestamp(x.Timestamp));
                        lvi.SubItems.Add(x.WindRPM);
                        lvi.SubItems.Add(x.Heat);
                        lvi.SubItems.Add(x.Mist);
                        SettingsListRun.Items.Add(lvi);
                    }
                    scheduler = new Scheduler(currentSettings, this);
                    ProgramTimer.Tick += new EventHandler(scheduler.ProgramTimer_Tick_Scheduler);
                    FileLabel.Text = Path.GetFileName(openFileDialog.FileName);
                    TimerButton.Enabled = true;
                    Countdown.Enabled = true;
                }
            }
        }

        private void WindRPMScrollbar_Scroll(object sender, EventArgs e)
        {
            WindRPMInput.Text = WindRPMScrollbar.Value.ToString();
        }

        private void Validate_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;

            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Enter)
                    {
                        nonNumberEntered = true;
                    }
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                HeatInput.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void Validate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered)
            {
                e.Handled = true;
            }
        }

        private void WindRPMInput_Leave(object sender, EventArgs e)
        {
            int rpm;
            if (!int.TryParse(WindRPMInput.Text, out rpm))
            {
                rpm = 0;
            }
            if (rpm > WindRPMScrollbar.Maximum)
            {
                rpm = WindRPMScrollbar.Maximum;
                WindRPMInput.Text = WindRPMScrollbar.Maximum.ToString();
            }
            WindRPMScrollbar.Value = rpm;
        }

        private void ScanForArduinosMenuItem_Click(object sender, EventArgs e)
        {
            ScanCOMPorts();
        }

        private void ConsoleWindow_TextChanged(object sender, EventArgs e)
        {
            RichTextBox box = (RichTextBox)sender;
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }

        #endregion
    }
}
