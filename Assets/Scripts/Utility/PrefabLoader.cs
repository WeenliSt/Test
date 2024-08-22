using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    //Взагалі уявимо що це сервіс який кешує конфігурації в ресурсах які потрібні проекту завжди, а додаткові матеріали ми витягаємо з адрессаблів
    public class PrefabLoader
    {
        public T LoadPrefabSync<T>(PrefabName prefabName, Transform parent = null) where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(prefabName.ToString());
            var prefabInstance = GameObject.Instantiate(prefab, parent);
            return prefabInstance;
        }
        
        public async Task<T> LoadAsyncPrefab<T>(PrefabName prefabName, Transform parent = null) where T : MonoBehaviour
        {
            var load = Resources.LoadAsync(prefabName.ToString());
            while(!load.isDone)
            {
                await Task.Yield();
            }
            var prefabInstance = Object.Instantiate(load.asset, parent);
            return prefabInstance.GetComponent<T>();
        }
    }
}