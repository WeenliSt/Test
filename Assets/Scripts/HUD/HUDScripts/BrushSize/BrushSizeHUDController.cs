using DefaultNamespace.Gameplay;
using DefaultNamespace.HUD.Core;

namespace DefaultNamespace.HUD.HUDScripts.BrushSize
{
    public class BrushSizeHUDController : BaseHUDController<BrushSizeHUDView>
    {

        public override PrefabName HUDItemName => PrefabName.HUDBrushSize;
        private BrushService _brushService;
        
        public BrushSizeHUDController(BrushService brushService)
        {
            _brushService = brushService;
        }
        protected override void OnShow()
        {
            base.OnShow();
            TargetView.DisableContainer();
            TargetView.SetInputText(string.Empty);
            TargetView.SetPlaceholderText("Current Size: " + _brushService.GetSize().ToString());
            TargetView.OnApplyButtonClick += OnApplyButtonClick;
            TargetView.OnMainButtonClick += OnMainButtonClick;
        }

        private void OnMainButtonClick()
        {
            TargetView.ToggleContainer();   
            TargetView.SetInputText(string.Empty);
        }

        private void OnApplyButtonClick(string text)
        {
            TargetView.ToggleContainer();
            _brushService.SetSize(text);
            TargetView.SetPlaceholderText("Current Size: " + _brushService.GetSize().ToString());
        }
    }
}