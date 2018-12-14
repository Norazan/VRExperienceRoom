using System;
using System.Collections.Generic;
using System.Linq;
using VRExperienceRoom.Serializables;
using System.IO.Ports;

namespace VRExperienceRoom
{
    /// <summary>
    /// Scheduler class that will keep track of the Device Settings and their corresponding timestamps, 
    /// then writing them to the corresponding COM port when the Program Timer hits each timestamp.
    /// </summary>
    public class Scheduler
    {
        private Form form;

        /// <summary>
        /// List of the DeviceSettings to be scheduled
        /// </summary>
        public List<DeviceSettings> settings { get; }

        /// <summary>
        /// Index keeping track of next setting to be scheduled
        /// </summary>
        public int settings_index { get; set; } = 0;

        /// <summary>
        /// Constructor for the scheduler. Takes a DeviceSettings list and the main thread Form class.
        /// </summary>
        /// <param name="_settings">DeviceSettings list to be scheduled.</param>
        /// <param name="_form">Reference to the main thread Formc class.</param>
        public Scheduler(List<DeviceSettings> _settings, Form _form)
        {
            settings = _settings;
            form = _form;
        }

        /// <summary>
        /// Tick event for the Program timer, handling the scheduling of the setting writes. 
        /// Calls <see cref="IOHandler.WriteSettings(SerialPort, string, bool, bool)"/> on each setting in the list when 
        /// the timer duration reaches the corresponding timestamp.
        /// </summary>
        /// <param name="sender">Controller object that fired the event.</param>
        /// <param name="e"><see cref="EventArgs"/> arguments.</param>
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
