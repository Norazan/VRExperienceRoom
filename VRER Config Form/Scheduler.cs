using System;
using System.Collections.Generic;
using System.Linq;
using VRExperienceRoom.Serializables;
using System.IO.Ports;

namespace VRExperienceRoom
{
    public class Scheduler : Form
    {
        public List<DeviceSettings> settings;
        public int settings_index = 0;

        public Scheduler(List<DeviceSettings> _settings)
        {
            settings = _settings;
        }

        public void ProgramTimer_Tick_Scheduler(object sender, EventArgs e)
        {
            if (!inCountdown)
            {
                TimeSpan duration = DateTime.Now - timerStart;
                if (settings_index < settings.Count)
                {
                    if (duration.TotalMilliseconds >= settings[settings_index].Timestamp)
                    {
                        SerialPort currentPort = IOHandler.Instance.ports.Where(x => x.PortName == settings[settings_index].Port).First();
                        if (!IOHandler.Instance.CheckExistingConnection(currentPort))
                        {
                            UpdateLogs(LogType.ERROR, "The following port cannot establish connection with Arduino: " + currentPort.PortName);
                        }
                        else
                        {
                            currentPort.Write(settings[settings_index].WindRPM + "\n"
                                + (settings[settings_index].Heat == "On" ? 1 : 0) + "\n"
                                + (settings[settings_index].Mist == "On" ? 1 : 0) + "\n");
                            settings_index++;
                        }
                    }
                }
            }
        }
    }
}
