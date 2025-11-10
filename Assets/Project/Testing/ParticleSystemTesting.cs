using Game.Project.ParticleSystemMgmt;
using Game.Utils.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Project
{
    public class ParticleSystemTesting : MonoBehaviour
    {
        [SerializeField] private ParticleSystemManager particleSystemManager;
        [SerializeField] private GameInput gameInput;

        // Start is called before the first frame update
        void Start()
        {
            gameInput.OnFirePressed += OnFire;
        }

        private void OnDestroy()
        {
            gameInput.OnFirePressed -= OnFire;
        }

        private void OnFire()
        {
            Vector2 worldPosition = gameInput.LookCamera.ScreenToWorldPoint(gameInput.GetLookPosition());
            particleSystemManager.Play(ParticleSystemType.Test, worldPosition);
        }
    }

}
