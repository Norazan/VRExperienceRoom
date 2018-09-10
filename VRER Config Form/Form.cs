using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			serialPort1.Write(textBox1.Text + '\n' + (checkBox1.Checked ? 1 : 0) + '\n' + (checkBox2.Checked ? 1 : 0) + '\n' + (checkBox3.Checked ? 1 : 0 ) + '\n' + (checkBox4.Checked ? 1 : 0) + '\n');
		}

		private void button2_Click(object sender, EventArgs e)
		{
			serialPort1.Write("*");
		}

		private void button3_Click(object sender, EventArgs e)
		{
			serialPort1.Write("-");
		}
	}
}
