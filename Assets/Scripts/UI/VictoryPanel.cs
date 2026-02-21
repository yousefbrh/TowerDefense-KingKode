using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class VictoryPanel : MonoBehaviour
    {
        [SerializeField] private Button victoryButton;
        
        public event Action OnNextLevelClicked;

        private void Start()
        {
            victoryButton.onClick.AddListener(VictoryButtonClicked);
        }

        private void VictoryButtonClicked()
        {
            OnNextLevelClicked?.Invoke();
        }
    }
}