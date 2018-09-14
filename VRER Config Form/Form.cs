﻿using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using VRERConfigForm.Serializables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

namespace VRER_Config_Form
{
    public partial class Form : System.Windows.Forms.Form
    {
        bool started = false;
        DateTime timerStart;
		public Form()
		{
			InitializeComponent();
            UpdateLogs("[Log] Connecting to Arduino...\n");
            string serialPort = "";
            foreach(string port in SerialPort.GetPortNames())
            {
                SerialPort sp = new SerialPort(port, 9600);
                sp.ReadTimeout = 500;
                try
                {
                    sp.Open();
                    sp.Write("@");
                    Thread.Sleep(500);
                    if(sp.ReadExisting() == "@")
                    {
                        serialPort = port;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
                finally
                {
                    sp.Close();
                }
            }
            if(serialPort == "")
            {
                UpdateLogs("[ERROR] Arduino not found.");
                throw new Exception("Arduino not found.");
            }
            else
            {
                serialPort1.PortName = serialPort;
                serialPort1.BaudRate = 9600;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);
                serialPort1.Open();
                UpdateLogs("[Log] Succesfully connected to Arduino.\n");
            }
		}

        private void sp1_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            string readString = serialPort1.ReadExisting();
            UpdateLogs(readString);
        }

        delegate void UpdateLogsCallback(string text);

        private void UpdateLogs(string text)
        {
            if (richTextBox1.InvokeRequired && richTextBox2.InvokeRequired && richTextBox3.InvokeRequired)
            {
                UpdateLogsCallback d = new UpdateLogsCallback(UpdateLogs);
                Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.AppendText(text);
                richTextBox2.AppendText(text);
                richTextBox3.AppendText(text);
            }
        }

        private string MsToTimestamp(string ms)
        {
            int _ms;
            if(!int.TryParse(ms, out _ms))
            {
                return "";
            }
            var seconds = _ms / 1000;
            var hours = seconds / 3600;
            seconds = seconds % 3600;
            var minutes = seconds / 60;
            seconds = seconds % 60;
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        private string TimestampToMs(string timestamp)
        {
            string[] splittedTimestamp = timestamp.Split(':');
            string s_Hours = splittedTimestamp[0];
            string s_Minutes = splittedTimestamp[1];
            string s_Seconds = splittedTimestamp[2];
            int i_hours;
            int i_minutes;
            int i_seconds;
            if(!int.TryParse(s_Hours, out i_hours) || !int.TryParse(s_Minutes, out i_minutes) || !int.TryParse(s_Seconds, out i_seconds))
            {
                return "";
            }
            return "" + (((i_hours * 3600) + (i_minutes * 60) + i_seconds) * 1000);
        }

		private void button1_Click(object sender, EventArgs e)
		{
            ListViewItem lvi = new ListViewItem(textBox1.Text.PadLeft(2, '0') + ":" + textBox2.Text.PadLeft(2, '0') + ":" + textBox3.Text.PadLeft(2, '0'));
            lvi.SubItems.Add(checkBox1.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox2.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox3.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox4.Checked ? "1" : "0");
            listView1.Items.Add(lvi);
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
                setting.Fan1 = x.SubItems[1].Text;
                setting.Fan2 = x.SubItems[2].Text;
                setting.Fan3 = x.SubItems[3].Text;
                setting.Fan4 = x.SubItems[4].Text;
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

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON File (*.json)|*.json";
            openFileDialog.Title = "Import  config file...";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    string json = sr.ReadToEnd();
                    List<DeviceSettings> settings = JsonConvert.DeserializeObject<List<DeviceSettings>>(json);
                    listView2.Items.Clear();
                    foreach(DeviceSettings x in settings)
                    {
                        ListViewItem lvi = new ListViewItem(MsToTimestamp(x.Timestamp));
                        lvi.SubItems.Add(x.Fan1);
                        lvi.SubItems.Add(x.Fan2);
                        lvi.SubItems.Add(x.Fan3);
                        lvi.SubItems.Add(x.Fan4);
                        listView2.Items.Add(lvi);
                    }
                    label2.Text = Path.GetFileName(openFileDialog.FileName);
                    button6.Enabled = true;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            serialPort1.Write("~");
            foreach(ListViewItem lvi in listView2.Items)
            {
                serialPort1.Write(TimestampToMs(lvi.SubItems[0].Text) + '\n' + lvi.SubItems[1].Text + '\n' + lvi.SubItems[2].Text + '\n' + lvi.SubItems[3].Text + '\n' + lvi.SubItems[4].Text + '\n');
            }
            listView2.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(started)
            {
                serialPort1.Write("-");
                timer1.Stop();
                label5.Text = "00:00.000";
                button7.Text = "Start";
                label4.Text = "Stopped";
                label4.ForeColor = System.Drawing.Color.Red;
                started = false;
            }
            else
            {
                timerStart = DateTime.Now;
                timer1.Start();
                serialPort1.Write("*");
                button7.Text = "Stop";
                label4.Text = "Started";
                label4.ForeColor = System.Drawing.Color.Green;
                started = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan duration = DateTime.Now - timerStart;
            label5.Text = duration.ToString("mm':'ss'.'fff");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.TextLength == 2)
            {
                textBox2.Select();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength == 2)
            {
                textBox3.Select();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Write((checkBox1.Checked ? "1" : "0") + (checkBox2.Checked ? "1" : "0") + (checkBox3.Checked ? "1" : "0") + (checkBox4.Checked ? "1" : "0") + '$');
        }
    }
}
