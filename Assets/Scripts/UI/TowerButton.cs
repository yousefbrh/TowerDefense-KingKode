using Components;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField] private TowerData towerData;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button button;

        private void Start()
        {
            if (towerData == null) return;

            if (iconImage != null && towerData.icon != null)
                iconImage.sprite = towerData.icon;

            if (costText != null)
                costText.text = $"${towerData.cost}";

            if (button != null)
                button.onClick.AddListener(OnClick);

            GameManager.Instance.onMoneyChanged.AddListener(UpdateButtonState);
            UpdateButtonState(GameManager.Instance.Money);
        }

        private void UpdateButtonState(int money)
        {
            if (button != null)
                button.interactable = money >= towerData.cost && !GameManager.Instance.IsGameOver;
        }

        private void OnClick()
        {
            TowerPlacer.Instance.SelectTower(towerData);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.onMoneyChanged.RemoveListener(UpdateButtonState);
        }
    }
}