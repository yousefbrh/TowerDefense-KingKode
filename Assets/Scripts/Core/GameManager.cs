using System;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public event Action<int> OnMoneyChanged;
        public event Action<int> OnLivesChanged;
        public event Action OnGameOver;
        
        [Header("Starting Values")]
        [SerializeField] private int startingMoney = 200;
        [SerializeField] private int startingLives = 20;

        public int Money  { get; private set; }
        public int Lives  { get; private set; }
        public bool IsGameOver { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); 
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Money = startingMoney;
            Lives = startingLives;
            OnMoneyChanged?.Invoke(Money);
            OnLivesChanged?.Invoke(Lives);
        }
        
        public bool TrySpendMoney(int amount)
        {
            if (amount > Money) return false;
            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
            return true;
        }

        public void AddMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
        }

        public void LoseLife(int amount = 1)
        {
            if (IsGameOver) return;
            Lives = Mathf.Max(0, Lives - amount);
            OnLivesChanged?.Invoke(Lives);

            if (Lives <= 0) TriggerGameOver();
        }

        private void TriggerGameOver()
        {
            IsGameOver = true;
            OnGameOver?.Invoke();
            Debug.Log("GAME OVER");
        }
    }
}
