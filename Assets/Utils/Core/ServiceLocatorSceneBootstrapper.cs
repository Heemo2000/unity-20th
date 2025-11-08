using UnityEngine;

namespace Game.Utils.Core
{
    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class ServiceLocatorSceneBootstrapper : Bootstrapper
    {
        protected override void Bootstrap()
        {
            base.Container.ConfigureForScene(); 
        }
    }
}
