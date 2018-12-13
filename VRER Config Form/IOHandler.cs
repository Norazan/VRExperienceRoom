using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace VRExperienceRoom
{
    public class IOHandler
    {
        private static IOHandler instance;

        public static IOHandler Instance {
            get
            {
                if(instance == null)
                {
                    instance = new IOHandler();
                }
                return instance;
            }
        }

        public List<SerialPort> ports = new List<SerialPort>();
        const int ThreadSleepLength = 50;
        const int BaudRate = 115200;

        public void ScanForArduinos()
        {
            CloseAllPorts();
            ports.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                try
                {
                    SerialPort sp = new SerialPort(port, BaudRate);
                    sp.Open();
                    sp.Write("@");
                    if (sp.ReadChar() == '@')
                    {
                        ports.Add(sp);
                    }
                    else
                    {
                        sp.Close();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public void WriteSettings(SerialPort port, string WindRPM, bool heat, bool scent)
        {
            char heat_char = heat ? '1' : '0';
            char scent_char = scent ? '1' : '0';
            port.Write(WindRPM + '\n' + heat_char + '\n' + scent_char + '\n');
        }

        public bool CheckExistingConnection(SerialPort port)
        {
            try
            {
                port.Write("@");
                if (port.ReadChar() == -1)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public void CloseAllPorts()
        {
            foreach(SerialPort p in ports)
            {
                try
                {
                    p.Close();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public void StopAllDevices()
        {
            foreach (SerialPort port in ports)
            {
                try
                {
                    port.Write("-");
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
