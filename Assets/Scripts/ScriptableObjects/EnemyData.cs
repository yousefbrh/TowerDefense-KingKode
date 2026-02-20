using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string enemyName = "Basic Enemy";
        public float maxHealth = 50f;
        public float moveSpeed = 3f;
        public int reward = 25;
        public int damage = 1;
    }
}