using DefaultNamespace.Gameplay.PainObject;
using DefaultNamespace.HUD.Core;

namespace DefaultNamespace.HUD.HUDScripts.ClearPaint
{
    public class ClearHUDController : BaseHUDController<ClearHUDView>
    {
        public ClearHUDController(PaintObjectService paintService)
        {
            _paintService = paintService;
        }

        public override PrefabName HUDItemName => PrefabName.HUDClear;
        private PaintObjectService _paintService;
        protected override void OnShow()
        {
            base.OnShow();
            TargetView.OnButtonClick += OnButtonClick;
        }

        protected override void OnHide()
        {
            base.OnHide();
            TargetView.OnButtonClick -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            _paintService.ClearAll(true);
        }
    }
}