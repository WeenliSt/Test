using DefaultNamespace.HUD.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.HUD.HUDScripts.HUDSave
{
    public class SaveHUDView : BaseHUDView
    {
        [SerializeField] private Button _button;
        
        public event UnityAction OnSaveButtonClicked
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }
    }
}