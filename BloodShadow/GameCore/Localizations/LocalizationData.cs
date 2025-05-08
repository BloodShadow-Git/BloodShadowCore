using System;

namespace BloodShadow.GameCore.Localizations
{
    [Serializable]
    public class LocalizationData
    {
        public string LocalizationKey { get; set; }
        public LocalizationPair[] Pairs { get; set; }

        public LocalizationData(string key)
        {
            LocalizationKey = key;
            Pairs = Array.Empty<LocalizationPair>();
        }
        public LocalizationData(string key, params LocalizationPair[] pairs) : this(key) { Pairs = pairs; }
    }
}
