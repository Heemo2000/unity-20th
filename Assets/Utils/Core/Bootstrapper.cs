using UnityEngine;

namespace Game.Utils.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        private ServiceLocator container;
        private bool hasBeenBootstrapped;
        internal ServiceLocator Container
        {
            get
            {
                if (container == null)
                {
                    container = GetComponent<ServiceLocator>();
                }

                return container;
            }
        }

        
        public void BootstrapOnDemand()
        {
            if(!hasBeenBootstrapped)
            {
                Bootstrap();
                hasBeenBootstrapped = true;
            }
        }

        protected abstract void Bootstrap();


        private void Awake()
        {
            BootstrapOnDemand();
        }


    }
}
