using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TowerButtonHandler towerButtonHandler;
        [SerializeField] private LosePanel losePanel;
        [SerializeField] private VictoryPanel victoryPanel;
        
        public event Action OnTryAgainClicked;
        public event Action OnNextLevelClicked;

        public static HUD Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            GameManager.Instance.onMoneyChanged.AddListener(UpdateMoney);
            GameManager.Instance.onLivesChanged.AddListener(UpdateLives);
            GameManager.Instance.onGameOver.AddListener(ShowGameOver);

            WaveManager.Instance.onWaveStarted.AddListener(UpdateWave);
            WaveManager.Instance.onAllWavesDone.AddListener(ShowVictory);

            if (losePanel != null)
            {
                losePanel.OnTryAgainClicked += TryAgainClicked;
                losePanel.gameObject.SetActive(false);
            }

            if (victoryPanel != null)
            {
                victoryPanel.OnNextLevelClicked += NextLevelClicked;
                victoryPanel.gameObject.SetActive(false);
            }

            UpdateMoney(GameManager.Instance.Money);
            UpdateLives(GameManager.Instance.Lives);
            UpdateWave(1);
        }
        
        private void NextLevelClicked()
        {
            OnNextLevelClicked?.Invoke();
        }

        private void TryAgainClicked()
        {
            OnTryAgainClicked?.Invoke();
        }

        public void InitTowerButtons(List<TowerData> towersData)
        {
            towerButtonHandler.InitTowerButtons(towersData);
        }

        private void UpdateMoney(int amount)
        {
            if (moneyText != null) moneyText.text = $"${amount}";
        }

        private void UpdateLives(int lives)
        {
            if (livesText != null) livesText.text = $"Lives: {lives}";
        }

        private void UpdateWave(int wave)
        {
            if (waveText != null) waveText.text = $"Wave {wave}";
        }

        private void ShowGameOver()
        {
            if (losePanel != null) losePanel.gameObject.SetActive(true);
        }

        private void ShowVictory()
        {
            if (victoryPanel != null) victoryPanel.gameObject.SetActive(true);
        }

        public void DeSelectAllTowerButtons()
        {
            towerButtonHandler.DeselectAllTowerButtons();
        }
    }
}