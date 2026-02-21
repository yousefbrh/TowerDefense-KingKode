using Entities;
using Managers;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace Components
{
    public class TowerPlacer : MonoBehaviour
    {
        public static TowerPlacer Instance { get; private set; }

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Camera mainCamera;

        private TowerData _selectedTowerData;
        private bool _isPlacing;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            if (mainCamera == null) mainCamera = Camera.main;
        }
        
        public void SelectTower(TowerData data)
        {
            if (GameManager.Instance.IsGameOver) return;
            if (!GameManager.Instance.CanAfford(data.cost))
            {
                Debug.Log("Not enough money!");
                return;
            }
            _selectedTowerData = data;
            _isPlacing = true;
        }

        private void CancelPlacement()
        {
            _selectedTowerData = null;
            _isPlacing = false;
            HUD.Instance.DeSelectAllTowerButtons();
        }

        private void Update()
        {
            if (!_isPlacing || !_selectedTowerData) return;
            if (GameManager.Instance.IsGameOver) { CancelPlacement(); return; }

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
            {
                var valid = GridManager.Instance.CanPlaceTower(hit.point);

                if (Input.GetMouseButtonDown(0) && valid)
                    PlaceTower(hit.point);
            }

            if (Input.GetMouseButtonDown(1)) CancelPlacement();
        }

        private void PlaceTower(Vector3 hitPoint, bool isUpgrade = false)
        {
            if (!isUpgrade)
            {
                if (!GameManager.Instance.CanAfford(_selectedTowerData.cost)) return;
                GridManager.Instance.PlaceTower(hitPoint);
                GameManager.Instance.SpendMoney(_selectedTowerData.cost);
            }

            var snapped = GridManager.Instance.SnapToGrid(hitPoint);
            var tower = Instantiate(_selectedTowerData.prefab, snapped, Quaternion.identity);
            tower.GetComponent<Tower>()?.Initialize(_selectedTowerData);
            
            CancelPlacement();
        }

        public void UpdateTower(Tower tower)
        {
            var data = tower.Data;
            _selectedTowerData = data.upgradedVersion;
            PlaceTower(tower.transform.position, true);
            Destroy(tower.gameObject);
        }
    }
}
