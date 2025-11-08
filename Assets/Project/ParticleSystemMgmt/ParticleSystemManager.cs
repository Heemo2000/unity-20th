using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils.Core;

namespace Game.Project.ParticleSystemMgmt
{
    public class ParticleSystemManager : MonoBehaviour
    {
        [SerializeField]private ParticleSystemData[] initializeData;
        
        private Dictionary<ParticleSystemType, ObjectPool<ParticleSystemHandler>> particlePools;
        
        private List<ParticleSystemHandler> activeParticles;
        private GameStateManager stateManager;

        public void Play(ParticleSystemType type, Vector3 position)
        {
            var pool = particlePools[type];
            ParticleSystemHandler particle = pool.Get();
            if (particle != null)
            {
                particle.SetPool(pool);
            }

            activeParticles.Add(particle);
            particle.transform.position = position;
            particle.Play();
        }

        private void Resume()
        {
            foreach (ParticleSystemHandler particleSystem in activeParticles)
            {
                particleSystem.Resume();
            }
        }

        private void Pause()
        {
            foreach(ParticleSystemHandler particleSystem in activeParticles)
            {
                particleSystem.Pause();
            }
        }

        public void Stop()
        {
            foreach(ParticleSystemHandler particleSystem in activeParticles)
            {
                particleSystem.Stop();
            }
        }

        private void HandleParticleState(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.UnPaused:
                                        Resume();
                                        break;
                case GameState.Paused:
                                        Pause();
                                        break;
            }
        }

        private void Awake()
        {
            particlePools = new Dictionary<ParticleSystemType, ObjectPool<ParticleSystemHandler>>();
            activeParticles = new List<ParticleSystemHandler>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            if(ServiceLocator.Global.TryGet<GlobalReferencesManager>(out GlobalReferencesManager globalReferencesManager))
            {
                stateManager = globalReferencesManager.GameStateManager;
            }
            if(stateManager != null)
            {
                stateManager.OnGameStateChanged.AddListener(HandleParticleState);
            }
            //GameStateManager.Instance.OnGameStateChanged.AddListener(HandleParticleState);
        }

        private void OnDestroy() 
        {
            if (stateManager != null)
            {
                stateManager.OnGameStateChanged.RemoveListener(HandleParticleState);
            }
            //GameStateManager.Instance.OnGameStateChanged.RemoveListener(HandleParticleState);
        }
    }
}
