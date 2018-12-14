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
    /// Main application class. Handles all GUI events and listeners.
    /// </summary>
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
        private delegate void UpdateLogsCallback(LogType logType, string text, string portName = null);
        private bool started = false;
        private bool nonNumberEntered = false;
        
        /// <summary>
        /// The exact <see cref="System.DateTime"/> stamp on which the timer was started. Used to calculate the duration of the timer.
        /// </summary>
        public DateTime timerStart;

        /// <summary>
        /// Boolean indicating if a countdown should be used when starting a timer.
        /// </summary>
        public bool inCountdown = false;


        /// <summary>
        /// Constructor for the main application.
        /// </summary>
        public Form()
		{
            Thread.CurrentThread.Name = "MainGUI";
            InitializeComponent();
            ScanCOMPorts();
        }

        /// <summary>
        /// Scans all available COM ports for available Arduino's and updates the GUI accordingly. 
        /// Checking of each COM port happens in the <see cref="IOHandler.ScanForArduinos"/> method.
        /// </summary>
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

        /// <summary>
        /// Adds a log line to the console windows. Since GUI elements can only be updated in the main thread, 
        /// it first does a check if the method was called from another thread. If so, run the method on the main thread through a delegate.
        /// </summary>
        /// <param name="logType">The type of log to be added. See <see cref="LogType"/></param>
        /// <param name="text">The text to be added as a log.</param>
        /// <param name="portName">The Serial Port name if the log to be added originates from an Arduino.</param>
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

        /// <summary>
        /// Stops the timer, updating all related GUI elements and sending the "Stop" console to all Arduino's with <see cref="IOHandler.StopAllDevices"/>.
        /// </summary>
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

        /// <summary>
        /// Send given settings to the corresponding Arduino. Writing of settings is done through <see cref="IOHandler.WriteSettings(SerialPort, string, bool, bool)"/>.
        /// Will first do a check if an Arduino is still available on the given COM port with <see cref="IOHandler.CheckExistingConnection(SerialPort)"/>.
        /// </summary>
        /// <param name="port">The COM port to send the settings through.</param>
        /// <param name="WindRPM">The RPM setting for the fans.</param>
        /// <param name="heat">The on or off setting for the heater.</param>
        /// <param name="scent">The on or off setting for the mist maker.</param>
        public void SendSettings(SerialPort port, string WindRPM, bool heat, bool scent)
        {
            if (!IOHandler.Instance.CheckExistingConnection(port))
            {
                UpdateLogs(LogType.ERROR, "The following port cannot establish connection with Arduino: " + port.PortName);
                if (ProgramTimer.Enabled)
                {
                    StopTimer();
                }
                ScanCOMPorts();
            }
            else
            {
                IOHandler.Instance.WriteSettings(port, WindRPM, heat, scent);
            }
        }

        #region Events and Listeners
        /// <summary>
        /// Listener for incoming data on COM ports. Will add a log to the console windows with <see cref="UpdateLogs(LogType, string, string)"/>.
        /// </summary>
        /// <param name="sender">Sender object of the incoming data.</param>
        /// <param name="args"><see cref="SerialDataReceivedEventArgs"/> arguments</param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string readString = sp.ReadLine();
            UpdateLogs(LogType.PORT, readString, sp.PortName);
        }

        /// <summary>
        /// Click event for the Add Settings button. Will add the entered device settings to the Settings List on the Create tab.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
		private void AddSettingButton_Click(object sender, EventArgs e)
		{
            //Range check the entered times
            string hours = HoursInput.Text.Contains("H") ? "0" : HoursInput.Text;
            string minutes = MinutesInput.Text.Contains("M") ? "0" : MinutesInput.Text;
            string seconds = SecondsInput.Text.Contains("S") ? "0" : SecondsInput.Text;

            if (int.Parse(hours) > 23)
                hours = "23";
            if (int.Parse(minutes) > 59)
                minutes = "59";
            if (int.Parse(seconds) > 59)
                seconds = "59";

            //Add all settings to a new ListViewItem object
            ListViewItem lvi = new ListViewItem(PortSelector.SelectedItem.ToString());
            lvi.SubItems.Add(hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0'));
            lvi.SubItems.Add(WindRPMInput.Text);
            lvi.SubItems.Add(HeatInput.Checked ? "On" : "Off");
            lvi.SubItems.Add(ScentInput.Checked ? "On" : "Off");

            //Check if item already exists with the same COM port and timestamp. In this case overwrite that item, else add it to the list and sort it
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

        /// <summary>
        /// Click event for the Remove Setting button. Remove all selected settings from the Settings List on the Create tab.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void RemoveSettingButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem x in SettingsListCreate.SelectedItems)
            {
                SettingsListCreate.Items.Remove(x);
            }
        }

        /// <summary>
        /// Click event for the Export button. Exports all settings from the Settings List on the Create tab to a JSON file in the chosen location.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void ExportButton_Click(object sender, EventArgs e)
        {
            //Create a list of DeviceSetting objects from all settings in the Settings List on the Create tab
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

            //Serialize the created list to a JSON string
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            //Create a new SaveFileDialog to select a location for the exported file, then write the JSON string to this file
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

        /// <summary>
        /// Click event for the Timer button. Will start or stop the timer and update all related GUI elements. 
        /// Stopping the timer is handled by <see cref="StopTimer"/>.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
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

        /// <summary>
        /// Tick event for the Program timer, handling all GUI related tasks. Updates the GUI based on the current duration of the timer.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
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

        /// <summary>
        /// Text Changed event for the Hours Input textfield. Selects Minutes Input if entered text is equal to the max length of the text box.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void HoursInput_TextChanged(object sender, EventArgs e)
        {
            if(HoursInput.TextLength == HoursInput.MaxLength)
            {
                MinutesInput.Select();
            }
        }

        /// <summary>
        /// Enter event for the Hours Input textfield. Removes placeholder.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void HoursInput_Enter(object sender, EventArgs e)
        {
            if(HoursInput.Text == "H")
            {
                HoursInput.Text = "";
                HoursInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// Leave event for the Hours Input textfield. Puts placeholder back if there is no input.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void HoursInput_Leave(object sender, EventArgs e)
        {
            if (HoursInput.Text == "")
            {
                HoursInput.Text = "H";
                HoursInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Text Changed event for the Minutes Input textfield. Selects Seconds Input if entered text is equal to the max length of the text box.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void MinutesInput_TextChanged(object sender, EventArgs e)
        {
            if (MinutesInput.TextLength == MinutesInput.MaxLength)
            {
                SecondsInput.Select();
            }
        }

        /// <summary>
        /// Enter event for the Minutes Input textfield. Removes placeholder.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void MinutesInput_Enter(object sender, EventArgs e)
        {
            if (MinutesInput.Text == "M")
            {
                MinutesInput.Text = "";
                MinutesInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// Leave event for the Minutes Input textfield. Puts placeholder back if there is no input.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void MinutesInput_Leave(object sender, EventArgs e)
        {
            if (MinutesInput.Text == "")
            {
                MinutesInput.Text = "M";
                MinutesInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Enter event for the Seconds Input textfield. Removes placeholder.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void SecondsInput_Enter(object sender, EventArgs e)
        {
            if (SecondsInput.Text == "S")
            {
                SecondsInput.Text = "";
                SecondsInput.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// Leave event for the Seconds Input textfield. Puts placeholder back if there is no input.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void SecondsInput_Leave(object sender, EventArgs e)
        {
            if (SecondsInput.Text == "")
            {
                SecondsInput.Text = "S";
                SecondsInput.ForeColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Click event for the Test Settings button. 
        /// Sends all currently entered settings to the corresponding COM port using <see cref="SendSettings(SerialPort, string, bool, bool)"/>.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void TestSettingsButton_Click(object sender, EventArgs e)
        {
            SerialPort currentPort = IOHandler.Instance.ports.Where(x => x.PortName == PortSelector.SelectedItem.ToString()).First();
            SendSettings(currentPort, WindRPMInput.Text, HeatInput.Checked, ScentInput.Checked);
        }

        /// <summary>
        /// Click event for the Import button. Imports all settings from a JSON configuration file to the Settings List in the Run tab.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void ImportButton_Click(object sender, EventArgs e)
        {
            //Create a new OpenFileDialog to select a JSON file to be imported.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON File (*.json)|*.json";
            openFileDialog.Title = "Import  config file...";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    //Read the JSON file into a string.
                    string json = sr.ReadToEnd();

                    //Deserialize the JSON string into a list of DeviceSettings.
                    List<DeviceSettings> currentSettings = JsonConvert.DeserializeObject<List<DeviceSettings>>(json);

                    //Add all DeviceSettings to the SettingsList in the Run tab
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

                    //Create a new Scheduler with the imported settings.
                    scheduler = new Scheduler(currentSettings, this);
                    ProgramTimer.Tick += new EventHandler(scheduler.ProgramTimer_Tick_Scheduler);

                    FileLabel.Text = Path.GetFileName(openFileDialog.FileName);
                    TimerButton.Enabled = true;
                    Countdown.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Scroll event for the Wind RPM scrollbar. Changes the Wind RPM input text to the scrollbar value.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void WindRPMScrollbar_Scroll(object sender, EventArgs e)
        {
            WindRPMInput.Text = WindRPMScrollbar.Value.ToString();
        }

        /// <summary>
        /// KeyDown event for Validation. Validates all text input to be a digit, except for Back and Enter.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="KeyEventArgs"/> arguments.</param>
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

        /// <summary>
        /// KeyPress event for Validation. Cancels the key press input if <see cref="nonNumberEntered"/> was 
        /// set to true by <see cref="Validate_KeyDown(object, KeyEventArgs)"/>.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="KeyPressEventArgs"/> arguments.</param>
        private void Validate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Leave event for the Wind RPM input. Sets the Wind RPM scrollbar to the entered text value.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
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

        /// <summary>
        /// Click event for the Scan for Arduino's menu button. Will re-scan all COM ports for Arduino's using <see cref="ScanCOMPorts"/>.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void ScanForArduinosMenuItem_Click(object sender, EventArgs e)
        {
            ScanCOMPorts();
        }

        /// <summary>
        /// TextChange event for the Console windows. Auto-scrolls to the bottom when new logs have been added.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
        private void ConsoleWindow_TextChanged(object sender, EventArgs e)
        {
            RichTextBox box = (RichTextBox)sender;
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }

        #endregion
    }
}
