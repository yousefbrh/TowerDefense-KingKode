using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTowerData", menuName = "TowerDefense/TowerData")]
    public class TowerData : ScriptableObject
    {
        [Header("Identity")]
        public string towerName = "Basic Tower";
        public Sprite icon;

        [Header("Stats")]
        public float damage = 10f;
        public float range = 5f;
        public float fireRate = 1f;
        public int cost = 100;

        [Header("Upgrade")]
        public float upgradeDamageBonus = 5f;
        public float upgradeRangeBonus = 1f;
        public int   upgradeCost = 75;

        [Header("Prefabs")]
        public GameObject towerPrefab;
        public GameObject projectilePrefab;
    }
}