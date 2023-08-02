using System;
using game_scene.models;
using UnityEngine;
using Zenject;

namespace game_scene.controllers {
public class CubeProvider : MonoBehaviour {
    [Inject(Id = PrefabId.Cube)] GameObject cubePrefab;
    [Inject] DiContainer diContainer;

    GameObject container;
    int cubeCount;

    void Awake() {
        container = new("cubes");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public Cube getCube(int number) {
        var cube = diContainer.InstantiatePrefabForComponent<Cube>(cubePrefab, container.transform);
        cube.name = $"cube{++cubeCount}_{number}";
        cube.setNumber(number);
        return cube;
    }
}
}