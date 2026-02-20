using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [SerializeField] private GameObject cellHighlightPrefab;
        [SerializeField] private List<GridSurface> surfaces = new List<GridSurface>();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private GridSurface GetSurface(Vector3 worldPos)
        {
            foreach (var surface in surfaces)
            {
                if (surface.WorldToCell(worldPos, out _))
                    return surface;
            }
            return null;
        }

        public bool CanPlaceTower(Vector3 worldPos)
        {
            var surface = GetSurface(worldPos);
            return surface != null && surface.CanPlace(worldPos);
        }

        public void PlaceTower(Vector3 worldPos)
            => GetSurface(worldPos)?.MarkOccupied(worldPos);

        public void RemoveTower(Vector3 worldPos)
            => GetSurface(worldPos)?.RemoveOccupied(worldPos);

        public Vector3 SnapToGrid(Vector3 worldPos)
            => GetSurface(worldPos)?.SnapToGrid(worldPos) ?? worldPos;
    }
}
