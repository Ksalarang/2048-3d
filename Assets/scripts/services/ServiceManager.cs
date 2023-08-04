using System.Collections.Generic;
using init_scene;
using services.saves;
using services.scenes;
using services.sounds;
using services.vibrations;
using UnityEngine;
using Utils;
using utils.interfaces;
using Zenject;

namespace services {
public class ServiceManager: MonoBehaviour {
    [Inject] LogConfig logConfig;
    
    [Inject] SceneService sceneService;
    [Inject] SoundService soundService;
    [Inject] VibrationService vibrationService;
    [Inject] SaveService saveService;

    Log log;
    List<AppLifecycleListener> appLifecycleListeners;
    List<SaveLoadListener> saveLoadListeners;

    void Awake() {
        log = new(GetType(), logConfig.serviceManager);
        appLifecycleListeners = new();
        saveLoadListeners = new();
        registerServices();
    }

    void registerServices() {
        log.log("register services");
        registerService(sceneService);
        registerService(soundService);
        registerService(vibrationService);
        registerService(saveService);
        onSavesLoaded();
    }

    void registerService(Service service) {
        if (service is AppLifecycleListener appLifecycleListener) {
            appLifecycleListeners.Add(appLifecycleListener);
        }
        if (service is SaveLoadListener saveLoadListener) {
            saveLoadListeners.Add(saveLoadListener);
        }
    }

    void onSavesLoaded() {
        log.log("on save loaded");
        var save = saveService.getSave();
        foreach (var listener in saveLoadListeners) {
            listener.onSaveLoaded(save);
        }
    }

    void OnApplicationPause(bool pauseStatus) {
        log.log("on pause");
        foreach (var listener in appLifecycleListeners) {
            listener.onPause();
        }
    }

    void OnApplicationQuit() {
        log.log("on quit");
        foreach (var listener in appLifecycleListeners) {
            listener.onQuit();
        }
    }
}

public interface Service {}
}