using System.Collections.Generic;
using DefaultNamespace.HUD.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.HUD.HUDScripts.HUDLoad
{
    public class LoadHUDView : BaseHUDView
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private GameObject _container;
        
        public string SelectedValue => _dropdown.options[_dropdown.value].text;
        public event UnityAction OnApplyButtonClicked
        {
            add => _applyButton.onClick.AddListener(value);
            remove => _applyButton.onClick.RemoveListener(value);
        }


        public void UpdateDropdownItem(List<TMP_Dropdown.OptionData> data)
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(data);
        }

        public void ToggleContainer(bool isEnabled) => _container.SetActive(isEnabled);
    }
}