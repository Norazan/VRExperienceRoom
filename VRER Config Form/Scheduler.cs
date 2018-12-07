using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRExperienceRoom.Serializables;
using System.IO.Ports;

namespace VRExperienceRoom
{
    public class Scheduler
    {
        protected List<DeviceSettings> settings;
        protected bool inCountdown = false;
        protected DateTime timerStart;
        private int settings_index = 0;

        public Scheduler(List<DeviceSettings> _settings)
        {
            settings = _settings;
        }

        public void ProgramTimer_Tick_Scheduler(object sender, EventArgs e)
        {
            TimeSpan duration = DateTime.Now - timerStart;
            if (settings_index < settings.Count)
            {
                if (duration.TotalMilliseconds >= settings[settings_index].Timestamp)
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
        }
    }
}
