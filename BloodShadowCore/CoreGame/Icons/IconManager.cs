namespace BloodShadow.CoreGame.Icons
{
    public class IconManager
    {
        public byte[] this[string device, string control]
        {
            get => _devices[device][control];
            set => _devices[device][control] = value;
        }
        public IDictionary<string, IconData> Devices => _devices;
        private readonly Dictionary<string, IconData> _devices;

        public IconManager() { _devices = []; }
        public IconManager(IconData[] datas) : this() { Add(datas); }
        public void Add(IconData data) { _devices[$"<{data.DeviceName}>"] = data; }
        public void Add(IconData[] datas) { foreach (IconData data in datas) { Add(data); } }

        public bool Get(string path, out byte[] icon)
        {
            string[] sep = path.Split('/');
            icon = null;
            if (_devices.TryGetValue(sep[0], out IconData data)) { if (data.DeviceIconsDictionary.TryGetValue(sep[1], out icon)) { return true; } }
            return false;
        }
    }
}
