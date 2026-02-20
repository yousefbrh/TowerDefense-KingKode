using Components;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class Tower : MonoBehaviour
    {
        public TowerData Data { get; private set; }

        [SerializeField] private Transform turretHead;
        [SerializeField] private Transform firePoint;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private TriggerInvoker detector;
        [SerializeField] private SphereCollider detectorCollider;

        private float _fireCooldown;
        private Enemy _currentTarget;
        private bool _isOccupied;

        public void Initialize(TowerData data)
        {
            Data = data;
            _isOccupied = false;
            detector.OnTriggerEnterInvoked += EnemyDetected;
            detector.OnTriggerExitInvoked += EnemyOffTheRadar;
            detectorCollider.radius = data.range;
        }
        
        private void EnemyDetected(Collider obj)
        {
            if (_isOccupied) return;
            var targetEnemy = obj.GetComponent<Enemy>();
            if (targetEnemy == null) return;
            _currentTarget = targetEnemy;
            _currentTarget.OnDied += CurrentTargetDied;
        }
        
        private void EnemyOffTheRadar(Collider obj)
        {
            var targetEnemy = obj.GetComponent<Enemy>();
            if (targetEnemy == null) return;
            targetEnemy.OnDied -= CurrentTargetDied;
            _currentTarget = null;
        }
        
        private void CurrentTargetDied(Enemy enemy)
        {
            enemy.OnDied -= CurrentTargetDied;
            _isOccupied = false;
            _currentTarget = null;
        }

        private void Update()
        {
            if (!Data || GameManager.Instance.IsGameOver) return;

            _fireCooldown -= Time.deltaTime;
            
            if (!_currentTarget) return;

            RotateTowardTarget();

            if (!(_fireCooldown <= 0f)) return;
            Fire();
            _fireCooldown = 1f / Data.fireRate;
        }

        private void RotateTowardTarget()
        {
            if (!turretHead) return;
            var dir = _currentTarget.transform.position - turretHead.position;
            dir.y = 0;
            if (dir != Vector3.zero)
                turretHead.rotation = Quaternion.LookRotation(dir);
        }

        private void Fire()
        {
            if (!Data.projectileData?.prefab) return;

            var spawnPos = firePoint ? firePoint.position : transform.position + Vector3.up;

            // muzzleFlash?.Play();

            var proj = ObjectPool.Instance.Spawn(Data.projectileData.projectileName, spawnPos, Quaternion.identity);
            if (!proj) return;
            
            var projectile = proj.GetComponent<Projectile>();
            projectile?.Initialize(Data.projectileData, _currentTarget);
        }

        public bool CanUpgrade() => Data.upgradedVersion != null
                                    && GameManager.Instance.CanAfford(Data.upgradeCost);

        public void Upgrade()
        {
            if (!CanUpgrade()) return;
            GameManager.Instance.SpendMoney(Data.upgradeCost);
            Initialize(Data.upgradedVersion);
        }

        private void OnDrawGizmosSelected()
        {
            if (Data == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, Data.range);
        }
    }
}
