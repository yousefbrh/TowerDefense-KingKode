using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Starting Values")]
        [SerializeField] private int startingMoney = 300;
        [SerializeField] private int startingLives = 20;
        
        [Header("Tower Data")]
        [SerializeField] private List<TowerData> towersData = new List<TowerData>();

        public int Money { get; private set; }
        public int Lives { get; private set; }
        public bool IsGameOver { get; private set; }

        public UnityEvent<int> onMoneyChanged = new UnityEvent<int>();
        public UnityEvent<int> onLivesChanged = new UnityEvent<int>();
        public UnityEvent onGameOver = new UnityEvent();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            Money = startingMoney;
            Lives = startingLives;
            onMoneyChanged.Invoke(Money);
            onLivesChanged.Invoke(Lives);
            HUD.Instance.InitTowerButtons(towersData);
            HUD.Instance.OnTryAgainClicked += TryAgainClicked;
            HUD.Instance.OnNextLevelClicked += NextLevelClicked;
        }

        private void TryAgainClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void NextLevelClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public bool CanAfford(int amount) => Money >= amount && !IsGameOver;

        public void SpendMoney(int amount)
        {
            Money -= amount;
            onMoneyChanged.Invoke(Money);
        }

        public void AddMoney(int amount)
        {
            Money += amount;
            onMoneyChanged.Invoke(Money);
        }

        public void LoseLife(int amount = 1)
        {
            Lives = Mathf.Max(0, Lives - amount);
            onLivesChanged.Invoke(Lives);
            if (Lives <= 0) TriggerGameOver();
        }

        private void TriggerGameOver()
        {
            IsGameOver = true;
            onGameOver.Invoke();
            Debug.Log("Game Over!");
        }
    }
}