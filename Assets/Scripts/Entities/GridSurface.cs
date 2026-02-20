using System;
using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Entities
{
    public class GridSurface : MonoBehaviour
    {
        [Header("Grid Settings")]
        public int columns = 10;
        public int rows = 10;
        public float cellSize = 1f;
        public string blockName;

        private Dictionary<Vector2Int, bool> _occupiedCells = new Dictionary<Vector2Int, bool>();
        private Dictionary<Vector2Int, bool> _invalidCells  = new Dictionary<Vector2Int, bool>();
        private Dictionary<Vector2Int, GameObject> _cellBlocks = new Dictionary<Vector2Int, GameObject>();

        private void Start()
        {
            SpawnAllBlocks();
        }
        
        private void SpawnAllBlocks()
        {
            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    var cell = new Vector2Int(c, r);
                    var worldPos = CellToWorld(cell);
                    var block = ObjectPool.Instance.Spawn(blockName, worldPos, transform.rotation);
                    block.transform.SetParent(transform);
                    block.SetActive(true);
                    _cellBlocks[cell] = block;
                }
            }
        }
        
        public bool WorldToCell(Vector3 worldPos, out Vector2Int cell)
        {
            var local = transform.InverseTransformPoint(worldPos);
            var col = Mathf.FloorToInt(local.x / cellSize);
            var row = Mathf.FloorToInt(local.z / cellSize);
            cell = new Vector2Int(col, row);
            return col >= 0 && col < columns && row >= 0 && row < rows;
        }

        private Vector3 CellToWorld(Vector2Int cell)
        {
            var local = new Vector3(
                cell.x * cellSize + cellSize * 0.5f,
                0f,
                cell.y * cellSize + cellSize * 0.5f);
            return transform.TransformPoint(local);
        }

        public bool CanPlace(Vector3 worldPos)
        {
            if (!WorldToCell(worldPos, out Vector2Int cell)) return false;
            return !_occupiedCells.ContainsKey(cell) && !_invalidCells.ContainsKey(cell);
        }

        public void MarkOccupied(Vector3 worldPos)
        {
            if (!WorldToCell(worldPos, out Vector2Int cell)) return;
            _occupiedCells[cell] = true;
            var block = _cellBlocks[cell];
            ObjectPool.Instance.ReturnToPool(blockName, block);
            _cellBlocks[cell] = null;
        }

        public void MarkInvalid(Vector3 worldPos)
        {
            if (WorldToCell(worldPos, out Vector2Int cell))
                _invalidCells[cell] = true;
        }

        public void RemoveOccupied(Vector3 worldPos)
        {
            if (!WorldToCell(worldPos, out Vector2Int cell)) return;
            _occupiedCells.Remove(cell);
            var block = ObjectPool.Instance.Spawn(blockName, worldPos, transform.rotation);
            _cellBlocks[cell] = block;
        }

        public Vector3 SnapToGrid(Vector3 worldPos)
        {
            if (WorldToCell(worldPos, out Vector2Int cell))
                return CellToWorld(cell);
            return worldPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0.8f, 0, 0.15f);
            for (int c = 0; c < columns; c++)
            for (int r = 0; r < rows; r++)
            {
                Vector3 center = CellToWorld(new Vector2Int(c, r));
                Gizmos.DrawWireCube(center, new Vector3(cellSize * 0.95f, 0.05f, cellSize * 0.95f));
            }
        }
    }
}
