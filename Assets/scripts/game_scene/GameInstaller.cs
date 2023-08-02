using game_scene.controllers;
using game_scene.input;
using UnityEngine;
using Zenject;

// ReSharper disable All

namespace game_scene {
public class GameInstaller : MonoInstaller {
    [Header("Game controller")]
    [SerializeField] GameController gameController;
    [SerializeField] GameSettings gameSettings;
    [Header("Controllers")]
    [SerializeField] CubeProvider cubeProvider;
    [SerializeField] CubeController cubeController;
    [SerializeField] InputController inputController;
    [Header("Objects")]
    [SerializeField] GameObject floor;
    [Header("Prefabs")]
    [SerializeField] GameObject cubePrefab;
    [Header("Misc")]
    [SerializeField] new Camera camera;
    
    public override void InstallBindings() {
        // game controller
        bind(gameController);
        // controllers
        bind(cubeProvider);
        bind(cubeController);
        bind(inputController);
        // objects
        bind(floor, ObjectId.Floor);
        // prefabs
        bind(cubePrefab, PrefabId.Cube);
        // settings
        bind(gameSettings.cubeSettings);
        // misc
        bind(camera);
    }
    
    void bind<T>(T instance) {
        Container.BindInstance(instance);
    }

    void bind<T>(T instance, object id) {
        Container.Bind<T>().WithId(id).FromInstance(instance);
    }
}

public enum ObjectId {
    Floor,
}

public enum PrefabId {
    Cube,
}
}