using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace VRExperienceRoom
{
    /// <summary>
    /// IO interface for writing commands through COM ports using UART. Also contains methods for scanning COM ports for Arduino's.
    /// </summary>
    public class IOHandler
    {
        private const int ThreadSleepLength = 50;
        private const int BaudRate = 115200;
        private static IOHandler instance;

        /// <summary>
        /// Singleton instance object for this class.
        /// </summary>
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

        /// <summary>
        /// Contains all COM ports that are connected to an Arduino.
        /// </summary>
        public List<SerialPort> ports = new List<SerialPort>();

        /// <summary>
        /// Loops through all available COM ports to check if an Arduino running the VR Experience Room software is available. 
        /// All comforming COM ports are added to the ports list.
        /// </summary>
        /// <remarks>
        /// The method checks for an Arduino by writing an '@' character to a COM port. If it receives an '@' character back, that means there is a Arduino attached.
        /// </remarks>
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

        /// <summary>
        /// Writes the given settings to a COM port. Should only be called using the <see cref="Form.SendSettings(SerialPort, string, bool, bool)"/> method!
        /// </summary>
        /// <remarks>
        /// The following communication protocol is used by the Arduino to interpret a setting through serial communication.
        /// <code>Wind RPM (0 - 5200) + '\n' + Heat Off/On (0/1) + '\n' + Scent Off/On (0/1) + '\n'</code>
        /// The following example serial string will turn the fans on to 1500 RPM, turn the heater On and scent Off.
        /// <code>1500\n1\n0\n</code>
        /// </remarks>
        /// <param name="port">The COM port to send the settings through.</param>
        /// <param name="WindRPM">The RPM setting for the fans.</param>
        /// <param name="heat">The on or off setting for the heater.</param>
        /// <param name="scent">The on or off setting for the mist maker.</param>s
        public void WriteSettings(SerialPort port, string WindRPM, bool heat, bool scent)
        {
            char heat_char = heat ? '1' : '0';
            char scent_char = scent ? '1' : '0';
            port.Write(WindRPM + '\n' + heat_char + '\n' + scent_char + '\n');
        }

        /// <summary>
        /// Checks the given COM port for an Arduino.
        /// </summary>
        /// <param name="port">The COM port to check for an Arduino.</param>
        /// <returns>False if there was no Arduino response, true if there was one.</returns>
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

        /// <summary>
        /// Closes all serial ports in the ports list.
        /// </summary>
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

        /// <summary>
        /// Send the stop command to all Arduino's, to stop all devices.
        /// </summary>
        /// <remarks>
        /// The command to stop all devices is a '-' character.
        /// </remarks>
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
