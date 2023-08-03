using System;
using System.Collections.Generic;
using game_scene.models;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Zenject;

namespace game_scene.controllers {
public class CubeProvider : MonoBehaviour {
    [Inject(Id = PrefabId.Cube)] GameObject cubePrefab;
    [Inject] DiContainer diContainer;

    Log log;
    GameObject container;
    List<Cube> cubes;
    ObjectPool<Cube> cubePool;
    int cubeCount;

    void Awake() {
        log = new Log(GetType());
        container = new("cubes");
        cubes = new List<Cube>();
        cubePool = new ObjectPool<Cube>(createCube, onCubeGet, onCubeRelease, onCubeDestroy, true, 32, 64);
    }

    #region cube pooling
    Cube createCube() {
        var cube = diContainer.InstantiatePrefabForComponent<Cube>(cubePrefab, container.transform);
        cubes.Add(cube);
        return cube;
    }

    void onCubeGet(Cube cube) {
        cube.reset();
        cube.gameObject.SetActive(true);
    }

    void onCubeRelease(Cube cube) {
        cube.state = CubeState.Destroyed;
        cube.gameObject.SetActive(false);
    }

    void onCubeDestroy(Cube cube) {
        Destroy(cube.gameObject);
    }
    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    public Cube getCube(long number) {
        var cube = cubePool.Get();
        cube.setNumber(number);
        cube.name = cube.ToString();
        return cube;
    }

    public void releaseCube(Cube cube) {
        if (cube.state != CubeState.Destroyed) cubePool.Release(cube);
    }

    public void reset() {
        foreach (var cube in cubes) {
            if (cube.state != CubeState.Destroyed) cubePool.Release(cube);
        }
    }
}
}