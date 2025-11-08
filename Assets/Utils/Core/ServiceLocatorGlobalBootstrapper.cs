using UnityEngine;

namespace Game.Utils.Core
{
    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class ServiceLocatorGlobalBootstrapper : Bootstrapper
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        protected override void Bootstrap()
        {
           base.Container.ConfigureAsGlobal(dontDestroyOnLoad); 
        }
    }
}
