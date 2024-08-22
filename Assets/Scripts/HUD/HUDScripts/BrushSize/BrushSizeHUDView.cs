using DefaultNamespace.HUD.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.HUD.HUDScripts.BrushSize
{
    public class BrushSizeHUDView : BaseHUDView
    {
        [SerializeField] private Button _mainButton;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TMP_Text _placeholderText;
        [SerializeField] private GameObject _inputContainer;
        [SerializeField] private Button _onApplyButtonClick;
        
        public event UnityAction<string> OnApplyButtonClick
        {
            add => _onApplyButtonClick.onClick.AddListener(() => value?.Invoke(_input.text));
            remove => _onApplyButtonClick.onClick.RemoveAllListeners();
        }
        public event UnityAction OnMainButtonClick
        {
            add => _mainButton.onClick.AddListener(value);
            remove => _mainButton.onClick.RemoveListener(value);
        }
        public void DisableContainer() => _inputContainer.SetActive(false);
        public void SetPlaceholderText(string text) => _placeholderText.text = text;

        public void ToggleContainer()
        {
            _inputContainer.SetActive(!_inputContainer.activeSelf);
        }

        public void SetInputText(string empty)
        {
            _input.text = empty;
        }
    }
}