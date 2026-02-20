using System.Collections;
using System.Collections.Generic;
using Components;
using Entities;
using Models;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Managers
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [SerializeField] private List<WaveModel> waves = new List<WaveModel>();
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private WaypointPath waypointPath;
        [SerializeField] private float timeBetweenWaves = 5f;

        public int CurrentWave { get; private set; } = 0;
        public int EnemiesAlive { get; private set; } = 0;

        public UnityEvent<int> onWaveStarted = new UnityEvent<int>();
        public UnityEvent onAllWavesDone = new UnityEvent();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => StartCoroutine(RunWaves());

        private IEnumerator RunWaves()
        {
            yield return new WaitForSeconds(2f);

            while (CurrentWave < waves.Count)
            {
                if (GameManager.Instance.IsGameOver) yield break;

                onWaveStarted.Invoke(CurrentWave + 1);
                yield return StartCoroutine(SpawnWave(waves[CurrentWave]));

                yield return new WaitUntil(() => EnemiesAlive <= 0);

                CurrentWave++;
                if (CurrentWave < waves.Count)
                    yield return new WaitForSeconds(timeBetweenWaves);
            }

            onAllWavesDone.Invoke();
        }

        private IEnumerator SpawnWave(WaveModel wave)
        {
            foreach (var group in wave.groups)
            {
                for (var i = 0; i < group.count; i++)
                {
                    SpawnEnemy(group.enemyData);
                    yield return new WaitForSeconds(group.spawnInterval);
                }
            }
        }

        private void SpawnEnemy(EnemyData data)
        {
            var obj = ObjectPool.Instance.Spawn(data.enemyName, spawnPoint.position, Quaternion.identity);
            if (!obj) return;

            var enemy = obj.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Initialize(data, waypointPath);
                enemy.OnDied += HandleEnemyDied;
                enemy.OnReachedGoal += HandleEnemyReachedGoal;
                EnemiesAlive++;
            }
        }

        private void HandleEnemyDied(Enemy enemy)
        {
            enemy.OnDied -= HandleEnemyDied;
            enemy.OnReachedGoal -= HandleEnemyReachedGoal;
            GameManager.Instance.AddMoney(enemy.Data.reward);
            EnemiesAlive--;
        }

        private void HandleEnemyReachedGoal(Enemy enemy)
        {
            enemy.OnDied -= HandleEnemyDied;
            enemy.OnReachedGoal -= HandleEnemyReachedGoal;
            GameManager.Instance.LoseLife(enemy.Data.damage);
            EnemiesAlive--;
        }
    }
}
