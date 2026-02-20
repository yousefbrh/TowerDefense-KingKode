using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        private float _maxHealth;
        private Camera _mainCam;

        private void Awake() => _mainCam = Camera.main;

        public void SetMaxHealth(float max)
        {
            _maxHealth = max;
            fillImage.fillAmount = 1f;
        }

        public void SetHealth(float current)
        {
            fillImage.fillAmount = Mathf.Clamp01(current / _maxHealth);
        }

        private void LateUpdate()
        {
            if (_mainCam != null)
                transform.LookAt(transform.position + _mainCam.transform.rotation * Vector3.forward,
                    _mainCam.transform.rotation * Vector3.up);
        }
    }
}