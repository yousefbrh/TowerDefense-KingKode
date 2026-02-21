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
        [SerializeField] private Button exitButton;
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
            exitButton?.onClick.AddListener(OnExit);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 200f, towerLayer))
            {
                var tower = hit.collider.GetComponentInParent<Tower>();
                if (tower && tower.CanShowUpgradePanel())
                {
                    ShowPanel(tower); return;
                }
            }

            HidePanel();
        }

        private void ShowPanel(Tower tower)
        {
            _selectedTower = tower;
            panel?.SetActive(true);

            var canUpgrade = tower.CanUpgrade();
            var canAfford = GameManager.Instance.CanAfford(tower.Data.upgradeCost);
            if (upgradeButton) upgradeButton.interactable = canUpgrade && canAfford;
            if (upgradeCostText)
            {
                if (canUpgrade && canAfford)
                    upgradeCostText.text = $"Upgrade ${tower.Data.upgradeCost}";
                if (canUpgrade && !canAfford)
                    upgradeCostText.text = $"Not Enough Money";
                if (!canUpgrade)
                    upgradeCostText.text = $"Max Level";
            }

            var sellValue = Mathf.RoundToInt(tower.Data.cost * 0.6f);
            if (sellAmountText) sellAmountText.text = $"Sell +${sellValue}";
        }

        private void HidePanel()
        {
            _selectedTower = null;
            panel?.SetActive(false);
        }

        private void OnUpgrade()
        {
            _selectedTower?.Upgrade();
            HidePanel();
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
        
        private void OnExit()
        {
            HidePanel();
        }
    }
}
