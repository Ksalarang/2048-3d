using System.Collections.Generic;
using game_scene.models;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using utils.structs;
using Zenject;
using Random = System.Random;

namespace game_scene.controllers {
public class CubeProvider : MonoBehaviour {
    [Inject(Id = PrefabId.Cube)] GameObject cubePrefab;
    [Inject(Id = ObjectId.StartLine)] GameObject startLine;
    [Inject] DiContainer diContainer;

    Log log;
    GameObject container;
    List<Cube> cubes;
    ObjectPool<Cube> cubePool;
    int cubeCount;
    ColorGenerator colorGenerator;
    FloatRange cubeSlowDownZRange;

    void Awake() {
        log = new Log(GetType());
        container = new("cubes");
        cubes = new List<Cube>();
        cubePool = new ObjectPool<Cube>(createCube, onCubeGet, onCubeRelease, onCubeDestroy, true, 32, 64);
        colorGenerator = new ColorGenerator();
        cubeSlowDownZRange = new FloatRange(
            startLine.transform.position.z + startLine.transform.localScale.z * 10 / 2 + 0.5f,
            1f
        );
    }

    #region cube pooling
    Cube createCube() {
        var cube = diContainer.InstantiatePrefabForComponent<Cube>(cubePrefab, container.transform);
        cube.setZRange(cubeSlowDownZRange);
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
        cube.setColor(colorGenerator.getColor(number));
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

    class ColorGenerator {
        readonly Dictionary<long, Color> colors = new();

        public ColorGenerator() {
            // generateColors();
        }

        void generateColors() {
            var random = new Random(42);
            for (var i = 1; i <= 30; i++) {
                var number = (long) Mathf.Pow(2, i);
                var color = new Color(
                    (float) random.NextDouble(),
                    (float) random.NextDouble(),
                    (float) random.NextDouble()
                );
                colors.Add(number, color);
            }
        }

        public Color getColor(long number) {
            if (colors.TryGetValue(number, out var color)) {
                return color;
            }
            var random = new Random(number.GetHashCode());
            float sum;
            do {
                color = new Color(
                    (float) random.NextDouble(),
                    (float) random.NextDouble(),
                    (float) random.NextDouble()
                );
                sum = color.r + color.g + color.b;
            } while (sum is < 0.3f or > 1.6f);
            colors.Add(number, color);
            return color;
        }
    }
}
}