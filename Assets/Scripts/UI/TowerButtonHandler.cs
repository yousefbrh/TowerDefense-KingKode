using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class TowerButtonHandler : MonoBehaviour
    {
        [SerializeField] private Transform towerButtonContainer;
        [SerializeField] private TowerButton towerButtonPrefab;
        
        private List<TowerButton> _towerButtons = new List<TowerButton>();
        
        public void InitTowerButtons(List<TowerData> towersData)
        {
            foreach (var towerData in towersData)
            {
                var clone = Instantiate(towerButtonPrefab, towerButtonContainer);
                clone.Initialize(towerData);
                clone.OnButtonClicked += ButtonClicked;
                _towerButtons.Add(clone);
            }
        }

        private void ButtonClicked()
        {
            DeselectAllTowerButtons();
        }

        public void DeselectAllTowerButtons()
        {
            foreach (var towerButton in _towerButtons)
            {
                towerButton.DeSelect();
            }
        }
    }
}