using Entities;
using Managers;
using ScriptableObjects;
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

        public void CancelPlacement()
        {
            _selectedTowerData = null;
            _isPlacing = false;
        }

        private void Update()
        {
            if (!_isPlacing || _selectedTowerData == null) return;
            if (GameManager.Instance.IsGameOver) { CancelPlacement(); return; }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
            {
                bool valid = GridManager.Instance.CanPlaceTower(hit.point);

                if (Input.GetMouseButtonDown(0) && valid)
                    PlaceTower(hit.point);
            }

            if (Input.GetMouseButtonDown(1)) CancelPlacement();
        }

        private void PlaceTower(Vector3 hitPoint)
        {
            if (!GameManager.Instance.CanAfford(_selectedTowerData.cost)) return;

            Vector3 snapped = GridManager.Instance.SnapToGrid(hitPoint);
            GameObject tower = Instantiate(_selectedTowerData.prefab, snapped, Quaternion.identity);
            tower.GetComponent<Tower>()?.Initialize(_selectedTowerData);

            GridManager.Instance.PlaceTower(hitPoint);
            GameManager.Instance.SpendMoney(_selectedTowerData.cost);

            CancelPlacement();
        }
    }
}
