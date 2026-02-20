using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewProjectileData", menuName = "TowerDefense/Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        public string projectileName = "Basic Bullet";
        public GameObject prefab;
        public float speed = 10f;
        public float damage = 10f;
    }
}