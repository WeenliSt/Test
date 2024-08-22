using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.HUD.Core
{
    public class HUDProvider
    {
        private PrefabLoader _prefabLoader;
        private HUDContainer _hudContainer;
        public HUDProvider()
        {
            _prefabLoader = new PrefabLoader();
        }
        public void InstantiateHUDContainer()
        {
            _hudContainer = _prefabLoader.LoadPrefabSync<HUDContainer>(PrefabName.HUDContainer);
            GameObject.DontDestroyOnLoad(_hudContainer.gameObject);
        }
        public async Task<BaseHUDView> InstantiateHUDView(PrefabName prefabName)
        {
            var task = _prefabLoader.LoadAsyncPrefab<BaseHUDView>(prefabName, _hudContainer.transform);
            await task;
            return task.Result;
        }
        public void Dispose()
        {
            GameObject.Destroy(_hudContainer.gameObject);
        }
    }
}