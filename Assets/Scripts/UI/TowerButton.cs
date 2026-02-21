using System;
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
        [SerializeField] private Image iconImage;
        [SerializeField] private Image stateImage;
        [SerializeField] private Image selectedImage;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button button;

        private TowerData _towerData;
        
        public event Action OnButtonClicked;

        public void Initialize(TowerData towerData)
        {
            if (towerData == null) return;

            _towerData = towerData;
            if (iconImage != null && _towerData.icon != null)
                iconImage.sprite = _towerData.icon;

            if (costText != null)
                costText.text = $"${_towerData.cost}";

            if (button != null)
                button.onClick.AddListener(OnClick);

            GameManager.Instance.onMoneyChanged.AddListener(UpdateButtonState);
            UpdateButtonState(GameManager.Instance.Money);
        }

        private void UpdateButtonState(int money)
        {
            if (button == null) return;
            var canBuy = money >= _towerData.cost && !GameManager.Instance.IsGameOver;
            button.interactable = canBuy;
            stateImage.color = canBuy ? Color.green : Color.red;
        }

        private void OnClick()
        {
            OnButtonClicked?.Invoke();
            var color = selectedImage.color;
            selectedImage.color = new Color(color.r, color.g, color.b, 1f);
            TowerPlacer.Instance.SelectTower(_towerData);
        }
        
        public void DeSelect()
        {
            var color = selectedImage.color;
            selectedImage.color = new Color(color.r, color.g, color.b, 0f);
        }

        private void OnDestroy()
        {
            OnButtonClicked = null;
            if (GameManager.Instance != null)
                GameManager.Instance.onMoneyChanged.RemoveListener(UpdateButtonState);
        }
    }
}