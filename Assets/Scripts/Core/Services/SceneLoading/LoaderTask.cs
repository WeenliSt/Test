using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class LoaderTask
    {
        public bool IsComplete { get; protected set; }
        public readonly float TargetProgress;

        protected LoaderTask(float targetProgress)
        {
            TargetProgress = targetProgress;
            IsComplete = false;
        }
    }

    public class SceneTask : LoaderTask
    {
        public SceneTask(float targetProgress) : base(targetProgress) { }

        public void Complete()
        {
            IsComplete = true;
        }
    }
    public class InitializationWaitTask : LoaderTask
    {
        public InitializationWaitTask(float targetProgress) : base(targetProgress) { }

        public void Complete()
        {
            IsComplete = true;
        }
    }

    public class WaitSecondTask : LoaderTask
    {
        private float _seconds;
        public WaitSecondTask(float seconds, float targetProgress) : base(targetProgress)
        {
            _seconds = seconds;
            StartWait();
        }

        private async void StartWait()
        {
            float x = _seconds;
            while (x  >  0)
            {
                x -= Time.deltaTime;
                await Task.Yield();
            }

            IsComplete = true;
        }
    }

    public class WaitHUDInstantiateTask : LoaderTask
    {
        public WaitHUDInstantiateTask(float targetProgress) : base(targetProgress)
        {
        }

        public void Complete()
        {
            IsComplete = true;
        }
    }
}