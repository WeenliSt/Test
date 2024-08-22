using DefaultNamespace.Gameplay.PainObject;
using DefaultNamespace.HUD.Core;

namespace DefaultNamespace.HUD.HUDScripts.HUDSave
{
    public class SaveHUDController : BaseHUDController<SaveHUDView>
    {
        private PaintObjectService _paintObjectService;

        public SaveHUDController(PaintObjectService paintObjectService)
        {
            _paintObjectService = paintObjectService;
        }

        public override PrefabName HUDItemName => PrefabName.HUDSave;

        protected override void OnShow()
        {
            base.OnShow();
            TargetView.OnSaveButtonClicked += SaveButtonClick;
        }

        protected override void OnHide()
        {
            base.OnHide();
            TargetView.OnSaveButtonClicked -= SaveButtonClick;
        }

        private void SaveButtonClick()
        {
            _paintObjectService.Save();
        }
    }
}