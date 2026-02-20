using ScriptableObjects;

namespace Models
{
    [System.Serializable]
    public class WaveConfig
    {
        public EnemyData enemyData;
        public int count = 5;
        public float spawnInterval = 1f;
    }
}