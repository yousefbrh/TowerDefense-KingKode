using System;
using UnityEngine;

namespace Components
{
    public class TriggerInvoker : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterInvoked;
        public event Action<Collider> OnTriggerStayInvoked;
        public event Action<Collider> OnTriggerExitInvoked;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterInvoked?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayInvoked?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitInvoked?.Invoke(other);
        }
    }
}