using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class CompositionRoot<TService> : MonoBehaviour where TService : IService
    {
        protected Dictionary<ServiceType, TService> _services = new();
        private ProjectCompositionRoot _projectCompositionRoot;

        protected ProjectCompositionRoot ProjectCompositionRoot
        {
            get
            {
                if(_projectCompositionRoot == null)
                {
                    _projectCompositionRoot = FindObjectOfType<ProjectCompositionRoot>();
                }

                return _projectCompositionRoot;
            }
        }
        protected void Awake()
        {
            
            ResolveServices();
            foreach (var service in _services)
            {
                service.Value.Initialize();
            }

            foreach (var service in _services)
            {
                service.Value.PostInitialize();
            }
            OnServicesPostInitialized();
        }

        protected abstract void ResolveServices();
        protected virtual void OnServicesPostInitialized()
        {
        }
        protected void AddService(ServiceType type, TService service)
        {
            _services.Add(type, service);
        }
        public  T GetService<T>(ServiceType type) where T : TService
        {
            return (T)_services[type];
        }
        
        private void OnDestroy()
        {
            foreach (var service in _services)
            {
                service.Value.Dispose();
            }
        }
    }
}