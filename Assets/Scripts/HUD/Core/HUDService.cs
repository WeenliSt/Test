using System.Collections.Generic;
using System.Threading.Tasks;

namespace DefaultNamespace.HUD.Core
{
    public class HUDService : IProjectService
    {
        private Dictionary<BaseHUDController, BaseHUDView> _hudMap = new();
        private HUDProvider _hudProvider;
        
        private SceneLoaderService _sceneLoaderService;
        private WaitHUDInstantiateTask _waitHUDInstantiateTask;
        
        private List<Task> _hudInstantiateTasks = new();
        public HUDService(SceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public void Initialize()
        {
            _sceneLoaderService.OnComplete += ShowHUD;
            
            _hudProvider = new HUDProvider();
            _hudProvider.InstantiateHUDContainer();
        }
        public void PostInitialize() { }
        public void Dispose() { }
        
        
        public void AddHUDElement(BaseHUDController controller)
        {
            _hudInstantiateTasks.Add(Map(controller, _hudProvider));
            CheckHUDInstantiateTasks();
        }

        private void CheckHUDInstantiateTasks()
        {
            if (_hudInstantiateTasks.Count == 1)
            {
                StartHUDInstantiateTasks();
            }
        }

        private async void StartHUDInstantiateTasks()
        {
            _sceneLoaderService.AddTask(_waitHUDInstantiateTask = new WaitHUDInstantiateTask(0.65f));
            await Task.WhenAll(_hudInstantiateTasks);
            _waitHUDInstantiateTask.Complete();
        }

        private void ShowHUD()
        {
            foreach (var controller in _hudMap.Keys)
            {
                controller.Show();
            }
        }

        private async Task Map(BaseHUDController controller, HUDProvider provider)
        {
            var task = controller.SetView(provider);
            await task;
            _hudMap.Add(controller, task.Result);
        }
    }
}