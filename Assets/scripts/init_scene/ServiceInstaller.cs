using services;
using services.saves;
using services.scenes;
using services.vibrations;
using UnityEngine;
using Zenject;

// ReSharper disable All

namespace init_scene {
public class ServiceInstaller : MonoInstaller {
    [SerializeField] GlobalConfig globalConfig;
    [SerializeField] ServiceManager serviceManager;
    
    public override void InstallBindings() {
        // settings
        bind(globalConfig);
        bind(globalConfig.logConfig);
        // services
        bind<SceneService, SceneService>();
        bind<VibrationService>();
        bind<SaveService, SimpleSaveService>();
        // service manager
        bind(serviceManager);
    }

    void bind<T>(T instance) {
        Container.BindInstance(instance);
    }

    void bind<T>() {
        Container.Bind<T>().FromNew().AsSingle().NonLazy();
    }

    void bind<Interface, Implementation>() where Implementation : Interface {
        Container.Bind<Interface>().To<Implementation>().AsSingle().NonLazy();
    }
}
}