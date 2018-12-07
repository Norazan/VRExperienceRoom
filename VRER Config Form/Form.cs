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
    /// <summary>
    /// 
    /// </summary>
    public partial class Form : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        /// </summary>
        enum LogType
        {
            LOG = 0,
            ERROR = 1,
            PORT = 2
        }

        const int MaxTimeCharacterInput = 2;

        static Scheduler scheduler = null;

        bool started = false;
        DateTime timerStart;
        //List<DeviceSettings> currentSettings = new List<DeviceSettings>();
        int currentSettings_index = 0;
        bool inCountdown = false;
        bool nonNumberEntered = false;
        //List<SerialPort> ports = new List<SerialPort>(); 

        delegate void UpdateLogsCallback(LogType logType, string text, string portName = null);

        public Form()
		{
			InitializeComponent();
            ScanCOMPorts();
		}

        private void ScanCOMPorts()
        {
            UpdateLogs(LogType.LOG, "Scanning COM ports for Arduino's...");

        }

        private void UpdateLogs(LogType logType, string text, string portName = null)
        {
            if (ConsoleWindowCreate.InvokeRequired && ConsoleWindowRun.InvokeRequired)
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

        #region Events and Listeners
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string readString = sp.ReadExisting();
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

            ListViewItem lvi = new ListViewItem(hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0'));
            lvi.SubItems.Add(WindRPMInput.Text);
            lvi.SubItems.Add(HeatInput.Checked ? "On" : "Off");
            lvi.SubItems.Add(ScentInput.Checked ? "On" : "Off");
            var existingLvi = SettingsListCreate.Items.Cast<ListViewItem>().FirstOrDefault(x => x.SubItems[0].Text == lvi.SubItems[0].Text);
            if (existingLvi == null)
            {
                SettingsListCreate.Items.Add(lvi);
                List<ListViewItem> tempCollection = SettingsListCreate.Items.Cast<ListViewItem>().OrderBy(x => DateTime.Parse(x.SubItems[0].Text).TimeOfDay).ToList();
                SettingsListCreate.Items.Clear();
                SettingsListCreate.Items.AddRange(tempCollection.ToArray());
            }
            else
            {
                SettingsListCreate.Items[existingLvi.Index] = lvi;
                lvi.Selected = true;
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
                setting.Timestamp = TimerUtil.TimestampToMs(x.SubItems[0].Text);
                setting.WindRPM = x.SubItems[1].Text;
                setting.Heat = x.SubItems[2].Text;
                setting.Mist = x.SubItems[3].Text;
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
                currentSettings_index = 0;
                foreach (SerialPort port in ports)
                {
                    port.Write("-");
                }  
                ProgramTimer.Stop();
                TimerLabel.Text = "00:00:00";
                TimerMS.Text = "000";
                TimerButton.Text = "Start";
                TimerStatus.Text = "Stopped";
                TimerStatus.ForeColor = System.Drawing.Color.Red;
                started = false;
            }
            else
            {
                timerStart = DateTime.Now;
                ProgramTimer.Start();
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
            SerialPort currentPort = ports.Where(x => x.PortName == PortSelector.SelectedText).First();
            currentPort.Write(WindRPMInput.Text + "\n" + (HeatInput.Checked ? "1" : "0") + "\n" + (ScentInput.Checked ? "1" : "0") + "\n");
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
                        ListViewItem lvi = new ListViewItem(TimerUtil.MsToTimestamp(x.Timestamp));
                        lvi.SubItems.Add(x.WindRPM);
                        lvi.SubItems.Add(x.Heat);
                        lvi.SubItems.Add(x.Mist);
                        SettingsListRun.Items.Add(lvi);
                    }
                    scheduler = new Scheduler(currentSettings);
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
            ScanForArduinos();
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
