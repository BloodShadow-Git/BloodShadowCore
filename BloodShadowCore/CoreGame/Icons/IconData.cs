namespace BloodShadow.CoreGame.Icons
{
    public class IconData
    {
        public byte[] this[string control]
        {
            get => _deviceIconsDictionary[control];
            set => _deviceIconsDictionary[control] = value;
        }

        public string DeviceName { get; private set; }
        public IDictionary<string, byte[]> DeviceIconsDictionary => _deviceIconsDictionary;
        private readonly Dictionary<string, byte[]> _deviceIconsDictionary;

        public IconData()
        {
            DeviceName = "EMPTY";
            _deviceIconsDictionary = [];
        }
        public IconData(string deviceName, IconPair[] pairs) : this()
        {
            DeviceName = deviceName;
            foreach (IconPair pair in pairs) { _deviceIconsDictionary[pair.Name] = pair.Icon; }
        }
        public IconData(IconPair[] pairs) : this() { foreach (IconPair pair in pairs) { _deviceIconsDictionary[pair.Name] = pair.Icon; } }


        public bool Add(IconPair pair)
        {
            if (pair.Icon != null) { return false; }
            _deviceIconsDictionary[pair.Name] = pair.Icon;
            return true;
        }

        public struct IconPair
        {
            public string Name { get; private set; }
            public byte[] Icon { get; private set; }

            public IconPair()
            {
                Name = "EMPTY";
                Icon = [];
            }

            public IconPair(string name, byte[] icon)
            {
                Name = name;
                Icon = icon;
            }
        }

        public static implicit operator Dictionary<string, byte[]>(IconData data)
        {
            if (data != null) { return data._deviceIconsDictionary; }
            else { return []; }
        }
    }
}
