using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRExperienceRoom.Util
{
    public static class TimerUtil
    {
        public static string MsToTimestamp(int ms)
        {
            var seconds = ms / 1000;
            var hours = seconds / 3600;
            seconds = seconds % 3600;
            var minutes = seconds / 60;
            seconds = seconds % 60;
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        public static int TimestampToMs(string timestamp)
        {
            string[] splittedTimestamp = timestamp.Split(':');
            string s_Hours = splittedTimestamp[0];
            string s_Minutes = splittedTimestamp[1];
            string s_Seconds = splittedTimestamp[2];
            int i_hours;
            int i_minutes;
            int i_seconds;
            if (!int.TryParse(s_Hours, out i_hours) || !int.TryParse(s_Minutes, out i_minutes) || !int.TryParse(s_Seconds, out i_seconds))
            {
                return -1;
            }
            return ((i_hours * 3600) + (i_minutes * 60) + i_seconds) * 1000;
        }
    }
}
