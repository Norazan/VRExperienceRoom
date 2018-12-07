using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace VRER_Config_Form
{
    public static class IOHandler
    {
        //protected List<SerialPort> ports = new List<SerialPort>();
        const int SerialPortReadTimeout = 500;
        const int BaudRate = 115200;

        public static List<SerialPort> ScanForArduinos()
        {
            List<SerialPort> ports = new List<SerialPort>();
            foreach (string port in SerialPort.GetPortNames())
            {
                SerialPort sp = new SerialPort(port, BaudRate);
                sp.ReadTimeout = SerialPortReadTimeout;
                sp.Open();
                sp.Write("@");
                Thread.Sleep(SerialPortReadTimeout);
                if (sp.ReadExisting() == "@")
                {
                    //sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    sp.ReadTimeout = SerialPort.InfiniteTimeout;
                    ports.Add(sp);
                }
                else
                {
                    sp.Close();
                }
            }

            if (ports.Count > 0)
            {
                string comports = "";
                foreach (SerialPort p in ports)
                {
                    comports += " " + p.PortName;
                }
                PortSelector.Items.Clear();
                PortSelector.Items.AddRange(ports.Select(x => x.PortName).ToArray());
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

        public static bool CheckExistingConnection(SerialPort port)
        {
            port.ReadTimeout = SerialPortReadTimeout;
            port.Write("@");
            Thread.Sleep(SerialPortReadTimeout);
            port.ReadTimeout = SerialPort.InfiniteTimeout;
            if (port.ReadExisting() == null)
                return false;
            return true;
        }
    }
}
