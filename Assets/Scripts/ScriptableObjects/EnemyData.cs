using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Identity")]
        public string enemyName = "Basic Enemy";

        [Header("Stats")]
        public float maxHealth = 100f;
        public float moveSpeed = 3f;
        public int reward = 25;
        public int damage = 1;

        [Header("Visuals")]
        public Color color = Color.red;
    }
}