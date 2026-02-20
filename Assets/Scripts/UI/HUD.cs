using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        private void Start()
        {
            GameManager.Instance.onMoneyChanged.AddListener(UpdateMoney);
            GameManager.Instance.onLivesChanged.AddListener(UpdateLives);
            GameManager.Instance.onGameOver.AddListener(ShowGameOver);

            WaveManager.Instance.onWaveStarted.AddListener(UpdateWave);
            WaveManager.Instance.onAllWavesDone.AddListener(ShowVictory);

            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (victoryPanel  != null) victoryPanel.SetActive(false);

            UpdateMoney(GameManager.Instance.Money);
            UpdateLives(GameManager.Instance.Lives);
            UpdateWave(1);
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
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        private void ShowVictory()
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
    }
}