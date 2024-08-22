using DefaultNamespace.HUD.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.HUD.HUDScripts.ClearPaint
{
    public class ClearHUDView : BaseHUDView
    {
        [SerializeField] private Button _button;
        
        public event UnityAction OnButtonClick
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        } 
    }
}