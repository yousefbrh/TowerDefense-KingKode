using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTowerData", menuName = "TowerDefense/Tower Data")]
    public class TowerData : ScriptableObject
    {
        [Header("General")]
        public string towerName = "Basic Tower";
        public Sprite icon;
        public GameObject prefab;
        public int cost = 100;

        [Header("Stats")]
        public float range = 5f;
        public float fireRate = 1f;

        [Header("Projectile")]
        public ProjectileData projectileData;

        [Header("Upgrade")]
        public TowerData upgradedVersion;
        public int upgradeCost = 150;
    }
}