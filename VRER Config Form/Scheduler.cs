using System;
using System.Collections.Generic;
using System.Linq;
using VRExperienceRoom.Serializables;
using System.IO.Ports;

namespace VRExperienceRoom
{
    public class Scheduler
    {
        public List<DeviceSettings> settings;
        public int settings_index = 0;
        private Form form;

        public Scheduler(List<DeviceSettings> _settings, Form _form)
        {
            settings = _settings;
            form = _form;
        }

        public void ProgramTimer_Tick_Scheduler(object sender, EventArgs e)
        {
            if (!form.inCountdown)
            {
                TimeSpan duration = DateTime.Now - form.timerStart;
                if (settings_index < settings.Count)
                {
                    if (duration.TotalMilliseconds >= settings[settings_index].Timestamp)
                    {
                        SerialPort currentPort = IOHandler.Instance.ports.Where(x => x.PortName == settings[settings_index].Port).First();
                        bool heat = settings[settings_index].Heat == "On";
                        bool scent = settings[settings_index].Mist == "On";
                        form.SendSettings(currentPort, settings[settings_index].WindRPM, heat, scent);
                        settings_index++;
                    }
                }
                else
                {
                    form.StopTimer();
                }
            }
        }
    }
}
