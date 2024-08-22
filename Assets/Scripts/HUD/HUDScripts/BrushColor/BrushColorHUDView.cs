using System;
using DefaultNamespace.HUD.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.HUD.HUDScripts.BrushColor
{
    public class BrushColorHUDView : BaseHUDView
    {
        [SerializeField] private FlexibleColorPicker _colorPicker;
        [SerializeField] private GameObject _colorPickerContainer;
        [SerializeField] private Button _mainButton;
        
        public event UnityAction OnMainButtonClick
        {
            add => _mainButton.onClick.AddListener(value);
            remove => _mainButton.onClick.RemoveListener(value);
        } 

        public event Action<Color> OnColorChange;
        private void OnEnable()
        {
            _colorPicker.onColorChange.AddListener(UpdateColor);
        }

        private void UpdateColor(Color color)
        {
            OnColorChange?.Invoke(color);
        }
        
        private void OnDisable()
        {
            _colorPicker.onColorChange.RemoveAllListeners();
        }

        public void SetInitialColor(Color color) => _colorPicker.color = color;

        public void ToggleColorPicker()
        {
            _colorPickerContainer.SetActive(!_colorPickerContainer.gameObject.activeSelf);
        }

        public void DisableTogglePicker()
        {
            _colorPickerContainer.SetActive(false);
        }
    }
}