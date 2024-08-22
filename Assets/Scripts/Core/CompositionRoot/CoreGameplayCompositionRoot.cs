using DefaultNamespace.Gameplay;
using DefaultNamespace.Gameplay.PainObject;
using DefaultNamespace.HUD.Core;
using DefaultNamespace.HUD.HUDScripts.BrushColor;
using DefaultNamespace.HUD.HUDScripts.BrushSize;
using DefaultNamespace.HUD.HUDScripts.ClearPaint;
using DefaultNamespace.HUD.HUDScripts.HUDLoad;
using DefaultNamespace.HUD.HUDScripts.HUDSave;
using UnityEngine;

namespace DefaultNamespace
{
    public class CoreGameplayCompositionRoot : CompositionRoot<IService>
    {
        [SerializeField] private Transform _paintObjectParent;
        private InitializationWaitTask InitializationWaitTask = new InitializationWaitTask(0.75f);

        protected override void ResolveServices()
        {
            ProjectCompositionRoot.GetService<SceneLoaderService>(ServiceType.SceneLoader).AddTask(InitializationWaitTask);
            AddService(ServiceType.BrushService, new BrushService());
            AddService(ServiceType.PaintObjectService, new PaintObjectService(GetService<BrushService>(ServiceType.BrushService), _paintObjectParent));
            RegisterHUD();
        }

        private void RegisterHUD()
        {
            ProjectCompositionRoot.GetService<HUDService>(ServiceType.HUD).AddHUDElement(new BrushSizeHUDController(GetService<BrushService>(ServiceType.BrushService)));
            ProjectCompositionRoot.GetService<HUDService>(ServiceType.HUD).AddHUDElement(new BrushColorHUDController(GetService<BrushService>(ServiceType.BrushService)));
            ProjectCompositionRoot.GetService<HUDService>(ServiceType.HUD).AddHUDElement(new SaveHUDController(GetService<PaintObjectService>(ServiceType.PaintObjectService)));
            ProjectCompositionRoot.GetService<HUDService>(ServiceType.HUD).AddHUDElement(new LoadHUDController(GetService<PaintObjectService>(ServiceType.PaintObjectService)));
            ProjectCompositionRoot.GetService<HUDService>(ServiceType.HUD).AddHUDElement(new ClearHUDController(GetService<PaintObjectService>(ServiceType.PaintObjectService)));
        }
        protected override void OnServicesPostInitialized()
        {
            InitializationWaitTask.Complete();
        }
    }
}