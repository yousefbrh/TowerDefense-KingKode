using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        [System.Serializable]
        public class Pool
        {
            public string itemTag;
            public GameObject prefab;
            public int initialSize = 10;
        }

        [SerializeField] private List<Pool> pools = new List<Pool>();

        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (var pool in pools)
            {
                var queue = new Queue<GameObject>();
                for (var i = 0; i < pool.initialSize; i++)
                {
                    var obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);
                    queue.Enqueue(obj);
                }
                _poolDictionary[pool.itemTag] = queue;
            }
        }

        public GameObject Spawn(string itemTag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.TryGetValue(itemTag, out var queue))
            {
                Debug.LogWarning($"ObjectPool: no pool with itemTag '{itemTag}'");
                return null;
            }

            GameObject obj;

            if (queue.Count > 0)
            {
                obj = queue.Dequeue();
            }
            else
            {
                var pool = pools.Find(p => p.itemTag == itemTag);
                obj = Instantiate(pool.prefab, transform);
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(string itemTag, GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            if (!_poolDictionary.ContainsKey(itemTag))
                _poolDictionary[itemTag] = new Queue<GameObject>();
            _poolDictionary[itemTag].Enqueue(obj);
        }
    }
}
