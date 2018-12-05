using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using VRERConfigForm.Serializables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace VRER_Config_Form
{
    public partial class Form : System.Windows.Forms.Form
    {
        enum LogType
        {
            LOG = 0,
            ERROR = 1,
            PORT = 2
        }

        const int SerialPortReadTimeout = 500;
        const int BaudRate = 115200;
        const int MaxTimeCharacterInput = 2;

        bool started = false;
        DateTime timerStart;
        List<DeviceSettings> currentSettings = new List<DeviceSettings>();
        int currentSettings_index = 0;
        bool inCountdown = false;
        bool nonNumberEntered = false;
        List<SerialPort> ports = new List<SerialPort>(); 

        delegate void UpdateLogsCallback(LogType logType, string text, string portName = null);

        public Form()
		{
			InitializeComponent();
            ScanForArduinos();
		}

        private void ScanForArduinos()
        {
            UpdateLogs(LogType.LOG, "Scanning COM ports for Arduino's...");
            foreach (string port in SerialPort.GetPortNames())
            {
                SerialPort sp = new SerialPort(port, BaudRate);
                sp.ReadTimeout = SerialPortReadTimeout;
                sp.Open();
                sp.Write("@");
                Thread.Sleep(SerialPortReadTimeout);
                if (sp.ReadExisting() == "@")
                {
                    sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    sp.ReadTimeout = SerialPort.InfiniteTimeout;
                    ports.Add(sp);
                }
                else
                {
                    sp.Close();
                }
            }

            if(ports.Count > 0)
            {
                string comports = "";
                foreach(SerialPort p in ports)
                {
                    comports += " " + p.PortName;
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(ports.Select(x => x.PortName).ToArray());
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = true;
                tabControl1.Enabled = true;
                UpdateLogs(LogType.LOG, "Connection established with Arduino's on the following COM ports:" + comports);
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("No Arduino's were found. Scan again");
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = false;
                tabControl1.Enabled = false;
                UpdateLogs(LogType.LOG, "No Arduino's could be found");
            }
        }

        private bool CheckExistingConnection(SerialPort port)
        {
            port.ReadTimeout = SerialPortReadTimeout;
            port.Write("@");
            Thread.Sleep(SerialPortReadTimeout);
            port.ReadTimeout = SerialPort.InfiniteTimeout;
            if (port.ReadExisting() == null)
                return false;
            return true;
        }

        private void UpdateLogs(LogType logType, string text, string portName = null)
        {
            if (richTextBox2.InvokeRequired && richTextBox3.InvokeRequired)
            {
                UpdateLogsCallback d = new UpdateLogsCallback(UpdateLogs);

                Invoke(d, new object[] { logType, text, portName });
            }
            else
            {
                switch (logType)
                {
                    case LogType.LOG:
                        richTextBox2.AppendText("[Log] " + text + "\n");
                        richTextBox3.AppendText("[Log] " + text + "\n");
                        break;
                    case LogType.ERROR:
                        richTextBox2.AppendText("[ERROR] " + text + "\n");
                        richTextBox3.AppendText("[ERROR] " + text + "\n");
                        break;
                    case LogType.PORT:
                        richTextBox2.AppendText("[" + portName + "] " + text);
                        richTextBox3.AppendText("[" + portName + "] " + text);
                        break;
                }
                
            }
        }

        private string MsToTimestamp(int ms)
        {
            var seconds = ms / 1000;
            var hours = seconds / 3600;
            seconds = seconds % 3600;
            var minutes = seconds / 60;
            seconds = seconds % 60;
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        private int TimestampToMs(string timestamp)
        {
            string[] splittedTimestamp = timestamp.Split(':');
            string s_Hours = splittedTimestamp[0];
            string s_Minutes = splittedTimestamp[1];
            string s_Seconds = splittedTimestamp[2];
            int i_hours;
            int i_minutes;
            int i_seconds;
            if (!int.TryParse(s_Hours, out i_hours) || !int.TryParse(s_Minutes, out i_minutes) || !int.TryParse(s_Seconds, out i_seconds))
            {
                return -1;
            }
            return ((i_hours * 3600) + (i_minutes * 60) + i_seconds) * 1000;
        }

        #region Events and Listeners
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            string readString = sp.ReadExisting();
            UpdateLogs(LogType.PORT, readString, sp.PortName);
        }

		private void button1_Click(object sender, EventArgs e)
		{
            string hours = textBox1.Text.Contains("H") ? "0" : textBox1.Text;
            string minutes = textBox2.Text.Contains("M") ? "0" : textBox2.Text;
            string seconds = textBox3.Text.Contains("S") ? "0" : textBox3.Text;

            if (int.Parse(hours) > 23)
                hours = "23";
            if (int.Parse(minutes) > 59)
                minutes = "59";
            if (int.Parse(seconds) > 59)
                seconds = "59";

            ListViewItem lvi = new ListViewItem(hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0'));
            lvi.SubItems.Add(textBox4.Text);
            lvi.SubItems.Add(checkBox1.Checked ? "On" : "Off");
            lvi.SubItems.Add(checkBox2.Checked ? "On" : "Off");
            var existingLvi = listView1.Items.Cast<ListViewItem>().FirstOrDefault(x => x.SubItems[0].Text == lvi.SubItems[0].Text);
            if (existingLvi == null)
            {
                listView1.Items.Add(lvi);
                List<ListViewItem> tempCollection = listView1.Items.Cast<ListViewItem>().OrderBy(x => DateTime.Parse(x.SubItems[0].Text).TimeOfDay).ToList();
                listView1.Items.Clear();
                listView1.Items.AddRange(tempCollection.ToArray());
            }
            else
            {
                listView1.Items[existingLvi.Index] = lvi;
                lvi.Selected = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem x in listView1.SelectedItems)
            {
                listView1.Items.Remove(x);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<DeviceSettings> settings = new List<DeviceSettings>();
            foreach(ListViewItem x in listView1.Items)
            {
                DeviceSettings setting = new DeviceSettings();
                setting.Timestamp = TimestampToMs(x.SubItems[0].Text);
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

        private void button7_Click(object sender, EventArgs e)
        {
            if(started)
            {
                currentSettings_index = 0;
                foreach (SerialPort port in ports)
                {
                    port.Write("-");
                }  
                timer1.Stop();
                label5.Text = "00:00:00";
                label3.Text = "000";
                button7.Text = "Start";
                label4.Text = "Stopped";
                label4.ForeColor = System.Drawing.Color.Red;
                started = false;
            }
            else
            {
                timerStart = DateTime.Now;
                timer1.Start();
                button7.Text = "Stop";
                label4.Text = "Started";
                label4.ForeColor = System.Drawing.Color.Green;
                started = true;
                if (checkBox5.Checked)
                {
                    inCountdown = true;
                    label4.Text = "Countdown";
                    label4.ForeColor = System.Drawing.Color.Yellow;
                    label5.ForeColor = System.Drawing.Color.Gray;
                    label3.ForeColor = System.Drawing.Color.Gray;
                } 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (inCountdown)
            {
                TimeSpan countdown = DateTime.Now - timerStart - new TimeSpan(0, 0, 5);
                if (countdown.TotalMilliseconds > 0)
                {
                    label4.Text = "Started";
                    label4.ForeColor = System.Drawing.Color.Green;
                    label5.ForeColor = System.Drawing.Color.Black;
                    label3.ForeColor = System.Drawing.Color.Black;
                    timerStart = DateTime.Now;
                    inCountdown = false;
                }
                label5.Text = countdown.ToString("hh':'mm':'ss");
                label3.Text = countdown.ToString("fff");
            }
            else
            {
                TimeSpan duration = DateTime.Now - timerStart;
                if (currentSettings_index < currentSettings.Count)
                {
                    if (duration.TotalMilliseconds >= currentSettings[currentSettings_index].Timestamp)
                    {
                        SerialPort currentPort = ports.Where(x => x.PortName == currentSettings[currentSettings_index].Port).First();
                        if (!CheckExistingConnection(currentPort))
                        {
                            UpdateLogs(LogType.ERROR, "The following port cannot establish connection with Arduino: " + currentPort.PortName);
                        }
                        else
                        {
                            currentPort.Write(currentSettings[currentSettings_index].WindRPM + "\n"
                                + (currentSettings[currentSettings_index].Heat == "On" ? 1 : 0) + "\n"
                                + (currentSettings[currentSettings_index].Mist == "On" ? 1 : 0) + "\n");
                            currentSettings_index++;
                        }
                    }
                }
                label5.Text = duration.ToString("hh':'mm':'ss");
                label3.Text = duration.ToString("fff");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.TextLength == textBox1.MaxLength)
            {
                textBox2.Select();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text == "H")
            {
                textBox1.Text = "";
                textBox1.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "H";
                textBox1.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength == textBox2.MaxLength)
            {
                textBox3.Select();
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "M")
            {
                textBox2.Text = "";
                textBox2.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "M";
                textBox2.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "S")
            {
                textBox3.Text = "";
                textBox3.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "S";
                textBox3.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SerialPort currentPort = ports.Where(x => x.PortName == comboBox1.SelectedText).First();
            currentPort.Write(textBox4.Text + "\n" + (checkBox1.Checked ? "1" : "0") + "\n" + (checkBox2.Checked ? "1" : "0") + "\n");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON File (*.json)|*.json";
            openFileDialog.Title = "Import  config file...";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    string json = sr.ReadToEnd();
                    currentSettings = JsonConvert.DeserializeObject<List<DeviceSettings>>(json);
                    listView2.Items.Clear();
                    foreach (DeviceSettings x in currentSettings)
                    {
                        ListViewItem lvi = new ListViewItem(MsToTimestamp(x.Timestamp));
                        lvi.SubItems.Add(x.WindRPM);
                        lvi.SubItems.Add(x.Heat);
                        lvi.SubItems.Add(x.Mist);
                        listView2.Items.Add(lvi);
                    }
                    label8.Text = Path.GetFileName(openFileDialog.FileName);
                    button7.Enabled = true;
                    checkBox5.Enabled = true;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = trackBar1.Value.ToString();
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
                checkBox1.Focus();
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

        private void textBox4_Leave(object sender, EventArgs e)
        {
            int rpm;
            if (!int.TryParse(textBox4.Text, out rpm))
            {
                rpm = 0;
            }
            if (rpm > trackBar1.Maximum)
            {
                rpm = trackBar1.Maximum;
                textBox4.Text = trackBar1.Maximum.ToString();
            }
            trackBar1.Value = rpm;
        }

        private void scanForArduinosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanForArduinos();
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox box = (RichTextBox)sender;
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }

        #endregion
    }
}
