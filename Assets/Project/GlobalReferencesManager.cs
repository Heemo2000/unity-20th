using UnityEngine;
using Game.Utils.Core;
using Game.Project.SoundManagement;

namespace Game.Project
{
    public class GlobalReferencesManager : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private GameInput gameInput;

        public SoundManager SoundManager { get => soundManager; }
        public GameStateManager GameStateManager { get => gameStateManager; }
        public GameInput GameInput { get => gameInput; }

        // Start is called before the first frame update
        void Start()
        {
            ServiceLocator.Global.Register<GlobalReferencesManager>(this);
        }
    }
}
