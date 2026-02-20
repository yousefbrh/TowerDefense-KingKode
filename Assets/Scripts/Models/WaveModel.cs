using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class WaveModel
    {
        public string waveName = "Wave 1";
        public List<WaveConfig> groups = new List<WaveConfig>();
    }
}