using System;
using UnityEngine;
using Zenject;

namespace Core
{
    public class UpdateTimer: IUpdatable
    {
        private bool _isStopped;

        public event Action OnTimerEnd;
        public event Action<float> OnTimeChanged;

        public float ElapsedTime { get; private set; }
        public float MaxTime { get; private set; }

        public bool IsTimerEnd => ElapsedTime >= MaxTime;

        public UpdateTimer()
        {
            Stop();
        }    

        [Inject]
        public UpdateTimer(ServiceUpdater serviceUpdater)
        {
            serviceUpdater.Subscribe(this);
            Stop();
        }    

        public UpdateTimer(float maxTime)
        {
            MaxTime = maxTime;
        }

        public void SetMaxTime(float maxTime)
        {
            ElapsedTime = maxTime*(ElapsedTime / MaxTime);
            MaxTime = maxTime;
        }

        public void Update()
        {
            if(_isStopped) return;
            AddTime();
        }

        public void Stop() => _isStopped = true;

        public void Continue() => _isStopped = false;

        public void Restart()
        {
            ElapsedTime = 0;
            Continue();
        }

        private void AddTime()
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > MaxTime)
            {
                ElapsedTime = MaxTime;
                Stop();
                OnTimerEnd?.Invoke();
            }
            else
            {
                OnTimeChanged?.Invoke(ElapsedTime);
            }
        }
    }
}