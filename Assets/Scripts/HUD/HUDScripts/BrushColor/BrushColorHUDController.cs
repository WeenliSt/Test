using DefaultNamespace.Gameplay;
using DefaultNamespace.HUD.Core;
using UnityEngine;

namespace DefaultNamespace.HUD.HUDScripts.BrushColor
{
    public class BrushColorHUDController : BaseHUDController<BrushColorHUDView>
    {
        public override PrefabName HUDItemName => PrefabName.HUDBrushColor;
        private BrushService _brushService;

        public BrushColorHUDController(BrushService brushService)
        {
            _brushService = brushService;
        }

        protected override void OnShow()
        {
            base.OnShow();
            TargetView.DisableTogglePicker();
            TargetView.OnColorChange += OnColorChange;
            TargetView.OnMainButtonClick += ToggleColorPicker;
            TargetView.SetInitialColor(_brushService.GetColor());
        }
        protected override void OnHide()
        {
            base.OnHide();
            TargetView.OnColorChange -= OnColorChange;
            TargetView.OnMainButtonClick -= ToggleColorPicker;
        }

        private void OnColorChange(Color color)
        {
            _brushService.SetColor(color);
        }
        private void ToggleColorPicker()
        {
            TargetView.ToggleColorPicker();
        }

    }
}