using DefaultNamespace.HUD.Core;

namespace DefaultNamespace
{
    public class ProjectCompositionRoot : CompositionRoot<IProjectService>
    {
        private InitializationWaitTask InitializationWaitTask = new InitializationWaitTask(0.25f);
        protected override void ResolveServices()
        {
            DontDestroyOnLoad(this.gameObject);
            var sceneService = new SceneLoaderService();
            AddService(ServiceType.SceneLoader, sceneService);
            AddService(ServiceType.HUD, new HUDService(sceneService));
        }

        protected override void OnServicesPostInitialized()
        { 
            var sceneService = GetService<SceneLoaderService>(ServiceType.SceneLoader);
            sceneService.LoadScene(RuntimeConstants.CORE_GAMEPLAY_SCENE_NAME);
            sceneService.AddTask(new WaitSecondTask(5, 1f));
            InitializationWaitTask.Complete();
        }
    }
}