namespace VRExperienceRoom.Serializables
{
    /// <summary>
    /// Serializabe class for every device setting. Also stores the corresponding COM port and timestamp.
    /// </summary>
    public class DeviceSettings
    {
        public string Port { get; set; }
        public int Timestamp { get; set; }
        public string WindRPM { get; set; }
        public string Heat { get; set; }
        public string Mist { get; set; }
    }
}
