using System.Threading.Tasks;

namespace DefaultNamespace.HUD.Core
{
    public abstract class BaseHUDController
    {
        public abstract PrefabName HUDItemName { get; }
        protected BaseHUDView View;
        public async Task<BaseHUDView> SetView(HUDProvider view)
        {
            var task = view.InstantiateHUDView(HUDItemName);
            await task;
            View = task.Result;
            OnSetView();
            View.gameObject.SetActive(false);
            return View;
        }
        
        protected abstract void OnSetView();
        public abstract void Show();
        public abstract void Hide();
    }
    public abstract class BaseHUDController<T> : BaseHUDController where T : BaseHUDView
    {
        protected T TargetView;
        
        protected sealed override void OnSetView()
        {
            TargetView = View as T;
        }

        public sealed override void Show()
        {
            TargetView.gameObject.SetActive(true);
            OnShow();
        }
        public sealed override void Hide()
        {
            TargetView.gameObject.SetActive(false);
            OnHide();
        }
        
        protected virtual void OnHide()
        {
            
        }
        protected virtual void OnShow()
        {
            
        }
    }
}