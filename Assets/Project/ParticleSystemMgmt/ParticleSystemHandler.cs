using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils.Core;

namespace Game.Project.ParticleSystemMgmt
{
    public class ParticleSystemHandler : MonoBehaviour
    {
        private ParticleSystem particleSystemComp;
        private ObjectPool<ParticleSystemHandler> pool;
        public ObjectPool<ParticleSystemHandler> Pool { get => pool;}

        public void SetPool(ObjectPool<ParticleSystemHandler> pool)
        {
            this.pool = pool;
        }

        public void Resume()
        {
            particleSystemComp.Play();
        }

        public void Play()
        {
            particleSystemComp.Play();
        }

        public void Pause()
        {
            particleSystemComp.Pause();
        }

        public void Stop()
        {
            particleSystemComp.Stop();
        }

        private void Awake()
        {
            particleSystemComp = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystemComp.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            if(this.pool != null)
            {
                this.pool.ReturnToPool(this);
            }
        }
    }
}
