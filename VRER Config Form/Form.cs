using System;
using System.Windows.Forms;
using System.IO;
using VRERConfigForm.Serializables;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VRER_Config_Form
{
    public partial class Form : System.Windows.Forms.Form
    { 

		public Form()
		{
			InitializeComponent();
			serialPort1.PortName = "COM4";
			serialPort1.BaudRate = 9600;
			serialPort1.Open();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            ListViewItem lvi = new ListViewItem(textBox1.Text);
            lvi.SubItems.Add(checkBox1.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox2.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox3.Checked ? "1" : "0");
            lvi.SubItems.Add(checkBox4.Checked ? "1" : "0");
            listView1.Items.Add(lvi);
            //serialPort1.Write(textBox1.Text + '\n' + (checkBox1.Checked ? 1 : 0) + '\n' + (checkBox2.Checked ? 1 : 0) + '\n' + (checkBox3.Checked ? 1 : 0 ) + '\n' + (checkBox4.Checked ? 1 : 0) + '\n');
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
                setting.Timestamp = x.SubItems[0].Text;
                setting.Fan1 = x.SubItems[1].Text;
                setting.Fan2 = x.SubItems[2].Text;
                setting.Fan3 = x.SubItems[3].Text;
                setting.Fan4 = x.SubItems[4].Text;
                settings.Add(setting);
            }
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.Title = "Export config file...";
            fileDialog.ShowDialog();

            if(fileDialog.FileName != "")
            {
                try
                {
                    File.WriteAllText(fileDialog.FileName, json);
                }catch(Exception ex)
                {
                    throw ex;
                }
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
