using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private ParticleSystem deathParticle;
        private float _currentHealth;
        private int _waypointIndex;
        private List<Transform> _waypoints = new List<Transform>();
        private HealthBar _healthBar;
        
        public EnemyData Data { get; private set; }
        public event Action<Enemy> OnDied;
        public event Action<Enemy> OnReachedGoal;
        
        public void Initialize(EnemyData data, WaypointPath waypointPath)
        {
            Data = data;
            _currentHealth = data.maxHealth;
            _waypointIndex = 0;

            if (healthBarPrefab && !_healthBar)
                _healthBar = Instantiate(healthBarPrefab, transform).GetComponent<HealthBar>();

            if (_healthBar)
                _healthBar.SetMaxHealth(data.maxHealth);

            _waypoints = waypointPath.waypointTransforms;
        }

        private void Update()
        {
            if (_waypoints == null || _waypoints.Count == 0 || GameManager.Instance.IsGameOver) return;

            MoveAlongPath();
        }

        private void MoveAlongPath()
        {
            var target = _waypoints[_waypointIndex];
            if (!target)
            {
                ObjectPool.Instance.ReturnToPool(Data.enemyName, gameObject);
                return;
            }
            var dir = (target.position - transform.position).normalized;
            transform.position += dir * (Data.moveSpeed * Time.deltaTime);

            // if (dir != Vector3.zero)
            //     transform.rotation = Quaternion.LookRotation(dir);

            var distanceSq = (transform.position - target.position).sqrMagnitude;
            if (distanceSq < 0.04f)
            {
                _waypointIndex++;
                if (_waypointIndex >= _waypoints.Count)
                    ReachGoal();
            }
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            _healthBar?.SetHealth(_currentHealth);

            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        { 
            PlayDeathParticle();
            OnDied?.Invoke(this);
            ReturnToPool();
        }

        private void PlayDeathParticle()
        {
            var clone = Instantiate(deathParticle, transform.position, Quaternion.identity);
            clone.Play();
            Destroy(clone.gameObject, deathParticle.main.duration);
        }

        private void ReachGoal()
        {
            OnReachedGoal?.Invoke(this);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            ObjectPool.Instance.ReturnToPool(Data.enemyName, gameObject);
        }
    }
}
