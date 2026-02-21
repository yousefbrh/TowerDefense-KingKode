using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LosePanel : MonoBehaviour
    {
        [SerializeField] private Button loseButton;
        
        public event Action OnTryAgainClicked;

        private void Start()
        {
            loseButton.onClick.AddListener(LoseButtonClicked);
        }

        private void LoseButtonClicked()
        {
            OnTryAgainClicked?.Invoke();
        }
    }
}