using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Utils.Core
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator global;
        private static Dictionary<Scene, ServiceLocator> sceneServiceLocators;
        private static List<GameObject> tempGameObjects;

        readonly ServiceManager serviceManager = new ServiceManager();

        const string GLOBAL_SERVICE_LOCATOR_NAME = "Service Locator[Global]";
        const string SCENE_SERVICE_LOCATOR_NAME = "Service Locator[Scene]";


        public static ServiceLocator Global
        {
            get
            {
                if(global != null)
                {
                    return global;
                }

                if(FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return global;
                }

                var container = new GameObject(GLOBAL_SERVICE_LOCATOR_NAME, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();

                return global;
            }
        }

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if(global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal : Already configured as global", this);
            }
            else if(global != null)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal : Another instance already configured as global");
            }
            else
            {
                global = this;
                if(dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if(sceneServiceLocators.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene : Another instance already configured for this scene!");
                return;
            }

            sceneServiceLocators.Add(scene, this);
        }

        public static ServiceLocator For(MonoBehaviour mb)
        {
            ServiceLocator locator = mb.GetComponentInParent<ServiceLocator>();
            if(locator != null)
            {
                return locator;
            }

            ServiceLocator sceneServiceLocator = ForSceneOf(mb);
            if(sceneServiceLocator != null)
            {
                return sceneServiceLocator;
            }

            return Global;
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if(sceneServiceLocators.TryGetValue(scene, out ServiceLocator locator) && locator != null)
            {
                return locator;
            }

            tempGameObjects.Clear();
            scene.GetRootGameObjects(tempGameObjects);

            var query = tempGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null);

            foreach(var go in query)
            {
                if(go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != null)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return Global;
        }

        public ServiceLocator Register<T>(T service) where T : class
        {
            serviceManager.Register(service);
            return this;
        }
        
        public ServiceLocator Register(Type type, object service)
        {
            serviceManager.Register(type, service);
            return this;
        }

        public bool TryGet<T>(out T service) where T : class
        {
            Get<T>(out service);
            if(service != null)
            {
                return true;
            }
            return false;
        }
        public ServiceLocator Get<T>(out T service) where T : class
        {
            if(TryGetService(out service))
            {
                return this;
            }

            if(TryGetNextInHierarchy(out ServiceLocator locator))
            {
                locator.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get : Service of type {typeof(T).FullName} is not registered!");
        }

        private bool TryGetService<T>(out T service) where T : class
        {
            return serviceManager.TryGet(out service);
        }

        private bool TryGetNextInHierarchy(out ServiceLocator locator)
        {
            if(this == global)
            {
                locator = null;
                return false;
            }

            locator = (transform.parent != null) ? transform.GetComponentInParent<ServiceLocator>() : null;
            if(locator == null)
            {
                locator = ForSceneOf(this);
            }

            return locator != null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatistics()
        {
            global = null;
            sceneServiceLocators = new Dictionary<Scene, ServiceLocator>();
            tempGameObjects = new List<GameObject>();
        }

        #if UNITY_EDITOR

        [MenuItem("GameObject/ServiceLocator/Add Global Instance")]
        static void AddGlobalInstance()
        {
            var go = new GameObject(GLOBAL_SERVICE_LOCATOR_NAME, typeof(ServiceLocatorGlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene Instance")]
        static void AddSceneInstance()
        {
            var go = new GameObject(SCENE_SERVICE_LOCATOR_NAME, typeof(ServiceLocatorSceneBootstrapper));
        }
#endif

        private void OnDestroy()
        {
            if(this == global)
            {
                global = null;
            }
            else if(sceneServiceLocators.ContainsValue(this))
            {
                sceneServiceLocators.Remove(gameObject.scene);
            }
        }
    }
}
