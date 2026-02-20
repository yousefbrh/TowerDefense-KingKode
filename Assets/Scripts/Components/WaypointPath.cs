using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Components
{
    public class WaypointPath : MonoBehaviour
    {
        [field: SerializeField] public List<Transform> waypointTransforms { get; private set; }

        private void OnDrawGizmos()
        {
            if (transform.childCount < 2) return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, 0.2f);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }
            Gizmos.DrawSphere(transform.GetChild(transform.childCount - 1).position, 0.3f);
        }
    }
}