using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    //Вирішив зробити, просто щоб було більше коду:)
    //Рішення погане, розмазане по проекту, проте не входить до логіки тестового завдання, тому було організовано так
    public class SceneLoaderService : IProjectService
    {
        private PrefabLoader _prefabLoader;
        private Scene _scene;
        private LaunchView _launchView;
        private SceneTask _sceneTask;
        private List<LoaderTask> _tasks = new();
        public Action OnComplete;
        public SceneLoaderService()
        {
            _prefabLoader = new PrefabLoader();
        }
        public void Initialize()
        {
            _launchView = _prefabLoader.LoadPrefabSync<LaunchView>(PrefabName.Splash);
            _launchView.ToggleContentHandler(false);
        }

        public void PostInitialize() {}
        public void Dispose() {}
        public void LoadScene(string sceneName)
        {
            _scene = SceneManager.GetActiveScene();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            AddTask(_sceneTask = new SceneTask(0.5f));
            _launchView.ToggleContentHandler(true);
            StartLoadingExecutor();
        }

        public void AddTask(LoaderTask task)
        {
            _tasks.Add(task);
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _sceneTask.Complete();
        }
        private async void StartLoadingExecutor()
        {
            _launchView.SetFillValue(0f);
            while (!_tasks.All(x => x.IsComplete))
            {
                var completedTasks = _tasks.Where(x => x.IsComplete).ToList();
                if(completedTasks.Any()) _launchView.SetFillValue(completedTasks.Max( x=> x.TargetProgress));
                await Task.Yield();
            }
            _launchView.ToggleContentHandler(false);
            _tasks.Clear();
            UnloadScene();
        }

        private void UnloadScene()
        {
            SceneManager.UnloadSceneAsync(_scene);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            OnComplete?.Invoke();
        }
    }
}