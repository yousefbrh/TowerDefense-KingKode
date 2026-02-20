using Components;
using Entities;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerUpgradeUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI upgradeCostText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button sellButton;
        [SerializeField] private TextMeshProUGUI sellAmountText;
        [SerializeField] private LayerMask towerLayer;
        [SerializeField] private Camera mainCamera;

        private Tower _selectedTower;

        private void Start()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            panel?.SetActive(false);
            upgradeButton?.onClick.AddListener(OnUpgrade);
            sellButton?.onClick.AddListener(OnSell);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            if (TowerPlacer.Instance != null) return;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, towerLayer))
            {
                var tower = hit.collider.GetComponentInParent<Tower>();
                if (tower != null) { ShowPanel(tower); return; }
            }

            HidePanel();
        }

        private void ShowPanel(Tower tower)
        {
            _selectedTower = tower;
            panel?.SetActive(true);

            bool canUpgrade = tower.CanUpgrade();
            if (upgradeButton != null) upgradeButton.interactable = canUpgrade;
            if (upgradeCostText != null)
                upgradeCostText.text = canUpgrade ? $"Upgrade ${tower.Data.upgradeCost}" : "Max Level";

            int sellValue = Mathf.RoundToInt(tower.Data.cost * 0.6f);
            if (sellAmountText != null) sellAmountText.text = $"Sell +${sellValue}";
        }

        private void HidePanel()
        {
            _selectedTower = null;
            panel?.SetActive(false);
        }

        private void OnUpgrade()
        {
            _selectedTower?.Upgrade();
            if (_selectedTower != null) ShowPanel(_selectedTower); // refresh
        }

        private void OnSell()
        {
            if (_selectedTower == null) return;
            var refund = Mathf.RoundToInt(_selectedTower.Data.cost * 0.6f);
            GridManager.Instance.RemoveTower(_selectedTower.transform.position);
            GameManager.Instance.AddMoney(refund);
            Destroy(_selectedTower.gameObject);
            HidePanel();
        }
    }
}
