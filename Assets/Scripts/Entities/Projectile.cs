using Components;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class Projectile : MonoBehaviour
    {
        private ProjectileData _data;
        private Enemy _target;
        private float _damage;

        [SerializeField] private ParticleSystem hitEffect;

        public void Initialize(ProjectileData projectileData, Enemy targetEnemy)
        {
            _data = projectileData;
            _target = targetEnemy;
            _damage = projectileData.damage;
        }

        private void Update()
        {
            if (!_target || !_target.gameObject.activeInHierarchy)
            {
                ReturnToPool();
                return;
            }

            var dir = (_target.transform.position - transform.position).normalized;
            transform.position += dir * (_data.speed * Time.deltaTime);

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
            
            if (Vector3.Distance(transform.position, _target.transform.position) < 0.3f)
                Hit();
        }

        private void Hit()
        {
            _target?.TakeDamage(_damage);

            // hitEffect?.Play();
            if (hitEffect)
                hitEffect.transform.SetParent(null);

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (!_data) return;
            ObjectPool.Instance.ReturnToPool(_data.projectileName, gameObject);
        }
    }
}