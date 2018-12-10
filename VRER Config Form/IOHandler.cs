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
            foreach (string port in SerialPort.GetPortNames())
            {
                SerialPort sp = new SerialPort(port, BaudRate);
                sp.Open();
                sp.Write("@");
                Thread.Sleep(ThreadSleepLength);
                if (sp.ReadExisting() == "@")
                {
                    ports.Add(sp);
                }
                else
                {
                    sp.Close();
                }
            }
        }

        public bool CheckExistingConnection(SerialPort port)
        {
            port.Write("@");
            Thread.Sleep(ThreadSleepLength);
            if (port.ReadExisting() == null)
                return false;
            return true;
        }

        public void StopAllDevices()
        {
            foreach (SerialPort port in ports)
            {
                port.Write("-");
            }
        }
    }
}
